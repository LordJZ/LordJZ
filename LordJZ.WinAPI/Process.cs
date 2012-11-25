using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LordJZ.Linq;
using LordJZ.WinAPI.Native;

namespace LordJZ.WinAPI
{
    public struct Process : IDisposable
    {
        #region Open

        public static Process Open(int pid)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Fields

        public static readonly Handle InvalidProcessHandle = Handle.NullHandle;

        readonly Handle m_handle;

        #endregion

        #region Properties

        public Handle Handle
        {
            get { return m_handle; }
        }

        public int Id
        {
            get
            {
                int id = UnsafeNativeMethods.GetProcessId(m_handle.Value);
                Win32Error.EnsureNoWin32Error(id != 0);
                return id;
            }
        }

        public bool IsWow64Process
        {
            get
            {
                bool result;
                UnsafeNativeMethods.IsWow64Process(m_handle.Value, out result)
                                   .EnsureNoWin32Error();
                return result;
            }
        }

        #endregion

        #region Constructors

        public Process(Handle handle)
        {
            Contract.Requires(handle != InvalidProcessHandle, "handle is invalid.");

            m_handle = handle;
        }

        #endregion

        #region Memory

        public int ReadMemory(IntPtr position, byte[] buffer, int offset, int count)
        {
            long read = ReadProcessMemory(m_handle.Value, position, buffer, offset, count);
            return checked((int)read);
        }

        public int WriteMemory(IntPtr position, byte[] buffer, int offset, int count)
        {
            long written = WriteProcessMemory(m_handle.Value, position, buffer, offset, count);
            return checked((int)written);
        }

        public Stream GetMemoryStream()
        {
            return new ProcessMemoryStream(m_handle);
        }

        #region Internal Helpers

        internal static unsafe long ReadProcessMemory(IntPtr handle, IntPtr position,
                                                      byte[] buffer, int offset, int count)
        {
            buffer.CheckBoundaries(offset, count);

            long read;
            fixed (byte* bufferPtr = buffer)
            {
                IntPtr ptr = new IntPtr(bufferPtr + offset);
                IntPtr ptrsizePosition = position;
                IntPtr ptrsizeCount = new IntPtr(count);
                IntPtr ptrsizeRead;

                UnsafeNativeMethods.ReadProcessMemory(handle, ptrsizePosition,
                                                      ptr, ptrsizeCount, out ptrsizeRead)
                                   .EnsureNoWin32Error();

                read = ptrsizeRead.ToInt64();
            }

            return read;
        }

        internal static unsafe long WriteProcessMemory(IntPtr handle, IntPtr position,
                                                       byte[] buffer, int offset, int count)
        {
            buffer.CheckBoundaries(offset, count);

            long read;
            fixed (byte* bufferPtr = buffer)
            {
                IntPtr ptr = new IntPtr(bufferPtr + offset);
                IntPtr ptrsizePosition = position;
                IntPtr ptrsizeCount = new IntPtr(count);
                IntPtr ptrsizeRead;

                UnsafeNativeMethods.WriteProcessMemory(handle, ptrsizePosition,
                                                       ptr, ptrsizeCount, out ptrsizeRead)
                                   .EnsureNoWin32Error();

                read = ptrsizeRead.ToInt64();
            }

            return read;
        }

        #endregion

        #endregion

        #region Termination

        public void Terminate(int exitCode)
        {
            UnsafeNativeMethods.TerminateProcess(m_handle.Value, (uint)exitCode)
                               .EnsureNoWin32Error();
        }

        public int ExitCode
        {
            get
            {
                int exitCode = this.AnyExitCode;

                if (exitCode == Constants.STILL_ACTIVE)
                    throw new InvalidOperationException("The process is not yet terminated.");

                return exitCode;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public int AnyExitCode
        {
            get
            {
                uint exitCode;

                UnsafeNativeMethods.GetExitCodeProcess(m_handle.Value, out exitCode)
                                   .EnsureNoWin32Error();

                return (int)exitCode;
            }
        }

        #endregion

        #region Modules

        unsafe bool EnumProcessModules(IntPtr[] array, out int nModules)
        {
            int nAllocModules = array.AllowNull().Select(_ => _.Length);
            int nAllocSpace = checked(nAllocModules * IntPtr.Size);
            int nSpace;

            IntPtr hProcess = m_handle.Value;

            bool result;
            fixed (IntPtr* pArray = array)
            {
                IntPtr arrayPtr = new IntPtr(pArray);
                result = UnsafeNativeMethods.EnumProcessModules(hProcess, arrayPtr, nAllocSpace, out nSpace);
            }

            if (!result && Win32Error.Last != Win32Error.PartialCopy)
                Win32Error.EnsureNoWin32Error(false);

            nModules = nSpace / IntPtr.Size;
            return result;
        }

        public ProcessModule[] GetModules()
        {
            Contract.Ensures(Contract.Result<ProcessModule[]>() != null);
            Contract.Ensures(Contract.Result<ProcessModule[]>().Length > 0);

            int nModules;
            int nAllocatedModules;

            EnumProcessModules(null, out nModules);

            IntPtr[] array;
            bool result;

            do
            {
                nAllocatedModules = (int)(nModules * 1.2);
                array = new IntPtr[nAllocatedModules];

                result = this.EnumProcessModules(array, out nModules);
            }
            while (result && nAllocatedModules >= nModules);

            ProcessModule[] modules = new ProcessModule[nModules];

            for (int i = 0; i < nModules; i++)
                modules[i] = new ProcessModule(m_handle, new Handle(array[i]));

            return modules;
        }

        #endregion

        #region Dispose helpers

        public bool Close()
        {
            return m_handle.Close();
        }

        void IDisposable.Dispose()
        {
            this.Close();
        }

        #endregion
    }
}
