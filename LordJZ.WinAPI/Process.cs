using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LordJZ.Linq;
using LordJZ.WinAPI.Native;

namespace LordJZ.WinAPI
{
    public struct Process : IDisposable
    {
        #region Open

        /// <summary>
        /// Opens an existing local process object.
        /// </summary>
        /// <param name="pid">
        /// The identifier of the local process to be opened.
        /// </param>
        /// <returns>
        /// Open handle to the specified process.
        /// </returns>
        public static Process Open(int pid)
        {
            return Open(pid, ProcessAccessRights.QueryLimitedInformation);
        }

        /// <summary>
        /// Opens an existing local process object.
        /// </summary>
        /// <param name="pid">
        /// The identifier of the local process to be opened.
        /// </param>
        /// <param name="rights">
        /// The access to the process object. This access right is checked against
        /// the security descriptor for the process. This parameter can be
        /// one or more of the <see cref="ProcessAccessRights"/>.
        /// </param>
        /// <returns>
        /// Open handle to the specified process.
        /// </returns>
        public static Process Open(int pid, ProcessAccessRights rights)
        {
            return Open(pid, rights, false);
        }

        /// <summary>
        /// Opens an existing local process object.
        /// </summary>
        /// <param name="pid">
        /// The identifier of the local process to be opened.
        /// </param>
        /// <param name="rights">
        /// The access to the process object. This access right is checked against
        /// the security descriptor for the process. This parameter can be
        /// one or more of the <see cref="ProcessAccessRights"/>.
        /// </param>
        /// <param name="inheritHandle">
        /// If this value is <c>true</c>, processes created by this process
        /// will inherit the handle. Otherwise, the processes do not inherit this handle.
        /// </param>
        /// <returns>
        /// Open handle to the specified process.
        /// </returns>
        public static Process Open(int pid, ProcessAccessRights rights, bool inheritHandle)
        {
            IntPtr result = UnsafeNativeMethods.OpenProcess((int)rights, inheritHandle, pid);

            Handle handle = new Handle(result);

            Win32Error.EnsureNoWin32Error(handle != InvalidProcessHandle);

            return new Process(handle);
        }

        public static Process Current
        {
            get { return new Process(new Handle(UnsafeNativeMethods.GetCurrentProcess())); }
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

        public ProcessPriority Priority
        {
            get
            {
                ProcessPriority ret = UnsafeNativeMethods.GetPriorityClass(m_handle.Value);
                (ret == default(ProcessPriority)).EnsureNoWin32Error();
                return ret;
            }
            set { UnsafeNativeMethods.SetPriorityClass(m_handle.Value, value).EnsureNoWin32Error(); }
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
            if (read != count)
                throw new Exception();
            return checked((int)read);
        }

        public byte[] ReadMemory(IntPtr position, int count)
        {
            byte[] buffer = new byte[count];
            ReadMemory(position, buffer, 0, count);
            return buffer;
        }

        public int ReadMemory(IntPtr position, IntPtr buffer, int count)
        {
            long read = ReadProcessMemory(m_handle.Value, position, buffer, count);
            if (read != count)
                throw new Exception();
            return checked((int)read);
        }

        public int WriteMemory(IntPtr position, byte[] buffer, int offset, int count)
        {
            long written = WriteProcessMemory(m_handle.Value, position, buffer, offset, count);
            if (written != count)
                throw new Exception();
            return checked((int)written);
        }

        public int WriteMemory(IntPtr position, IntPtr buffer, int count)
        {
            long written = WriteProcessMemory(m_handle.Value, position, buffer, count);
            if (written != count)
                throw new Exception();
            return checked((int)written);
        }

        public string ReadCString(IntPtr position, int sizeFactor = 64)
        {
            return ReadCString(Encoding.ASCII, true, position, sizeFactor);
        }

        public string ReadCString(Encoding encoding, bool singleByteTerminator, IntPtr position, int sizeFactor = 64)
        {
            if (sizeFactor <= 0)
                throw new ArgumentOutOfRangeException("sizeFactor");

            int index = 0;
            byte[] array = new byte[sizeFactor];
            while (true)
            {
                int read;
                while (!TryReadProcessMemory(m_handle.Value, position, array, index, sizeFactor, out read))
                {
                    sizeFactor = checked(sizeFactor / 2);
                    if (sizeFactor == 0)
                        throw new Win32Exception(Win32Error.Last);
                }

                int terminatorIdx;
                if (singleByteTerminator)
                    terminatorIdx = FindZeroByte(array, index, sizeFactor);
                else
                    terminatorIdx = FindTwoZeroBytes(array, index, sizeFactor);

                if (terminatorIdx >= 0)
                    return encoding.GetString(array, 0, terminatorIdx);

                index = checked(index + read);
                position += read;

                if (array.Length - index - sizeFactor < 0)
                    Array.Resize(ref array, checked(array.Length * 2));
            }
        }

        public unsafe T ReadStructure<T>(IntPtr position) where T : struct
        {
            Type structType = typeof(T);
            int size = Marshal.SizeOf(structType);

            byte[] memory = new byte[size];
            ReadMemory(position, memory, 0, size);

            fixed (byte* structPtr = memory)
                return (T)Marshal.PtrToStructure((IntPtr)structPtr, structType);
        }

        /// <summary>
        /// Creates the <see cref="Stream"/> whose backing storage is
        /// the memory space of this instance of <see cref="Process"/>.
        /// </summary>
        /// <returns>
        /// A instance of <see cref="Stream"/> whose backing storage is
        /// the memory space of this instance of <see cref="Process"/>.
        /// </returns>
        public Stream GetMemoryStream()
        {
            return new ProcessMemoryStream(m_handle);
        }

        #region Internal Helpers

        internal static unsafe int ReadProcessMemory(IntPtr handle, IntPtr position,
                                                      byte[] buffer, int offset, int count)
        {
            buffer.CheckBoundaries(offset, count);

            fixed (byte* bufferPtr = buffer)
                return ReadProcessMemory(handle, position, (IntPtr)(bufferPtr + offset), count);
        }

        internal static int ReadProcessMemory(IntPtr handle, IntPtr position, IntPtr ptr, int count)
        {
            IntPtr ptrsizeCount = new IntPtr(count);
            IntPtr ptrsizeRead;

            UnsafeNativeMethods.ReadProcessMemory(handle, position,
                                                  ptr, ptrsizeCount, out ptrsizeRead)
                               .EnsureNoWin32Error();

            return ptrsizeRead.ToInt32();
        }

        internal static unsafe bool TryReadProcessMemory(IntPtr handle, IntPtr position,
                                                         byte[] buffer, int offset, int count,
                                                         out int read)
        {
            buffer.CheckBoundaries(offset, count);

            fixed (byte* bufferPtr = buffer)
                return TryReadProcessMemory(handle, position, (IntPtr)(bufferPtr + offset), count, out read);
        }

        internal static bool TryReadProcessMemory(IntPtr handle, IntPtr position, IntPtr ptr, int count, out int read)
        {
            IntPtr ptrsizeCount = new IntPtr(count);
            IntPtr ptrsizeRead;

            bool result = UnsafeNativeMethods.ReadProcessMemory(handle, position,
                                                                ptr, ptrsizeCount, out ptrsizeRead);

            read = ptrsizeRead.ToInt32();
            return result && read == count;
        }

        internal static unsafe long WriteProcessMemory(IntPtr handle, IntPtr position,
                                                       byte[] buffer, int offset, int count)
        {
            buffer.CheckBoundaries(offset, count);

            fixed (byte* bufferPtr = buffer)
                return WriteProcessMemory(handle, position, (IntPtr)(bufferPtr + offset), count);
        }

        internal static long WriteProcessMemory(IntPtr handle, IntPtr position, IntPtr buffer, int count)
        {
            IntPtr read;

            UnsafeNativeMethods.WriteProcessMemory(handle, position,
                                                   buffer, (IntPtr)count, out read)
                               .EnsureNoWin32Error();

            return read.ToInt64();
        }

        static int FindZeroByte(byte[] array, int startIndex, int count)
        {
            return Array.IndexOf(array, (byte)0, startIndex, count);
        }

        static int FindTwoZeroBytes(byte[] array, int startIndex, int count)
        {
            Contract.Requires(array != null);
            Contract.Requires(startIndex >= 0);
            Contract.Requires(startIndex <= array.Length);
            Contract.Requires(count >= 0);
            Contract.Requires(count <= array.Length - startIndex);

            for (int i = startIndex, l = startIndex + count - 1; i < l; i++)
            {
                if (array[i] == 0 && array[i + 1] == 0)
                    return i;
            }

            return -1;
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

                if (exitCode == Win32Error.StillActive)
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

        public ProcessModule[] GetModules()
        {
            Contract.Ensures(Contract.Result<ProcessModule[]>() != null);
            Contract.Ensures(Contract.Result<ProcessModule[]>().Length > 0);

            int nModules;
            IntPtr[] array = GetModules(out nModules);

            ProcessModule[] modules = new ProcessModule[nModules];

            for (int i = 0; i < nModules; i++)
                modules[i] = new ProcessModule(m_handle, new Handle(array[i]));

            return modules;
        }

        public ProcessModule MainModule
        {
            get
            {
                int count;
                IntPtr[] array = GetModules(out count);
                if (count == 0)
                    throw new InvalidOperationException("There are no modules in this process.");

                return new ProcessModule(this, new Handle(array[0]));
            }
        }

        #region Internal helpers

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

        IntPtr[] GetModules(out int count)
        {
            Contract.Ensures(Contract.Result<IntPtr[]>() != null);
            Contract.Ensures(Contract.ValueAtReturn(out count) >= 0);
            Contract.Ensures(Contract.Result<IntPtr[]>().Length >= Contract.ValueAtReturn(out count));

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
            while (!(result && nAllocatedModules >= nModules));

            count = nModules;
            return array;
        }

        #endregion

        #endregion

        #region DpiAwareness

        /// <summary>
        /// Sets the current process's DPI awareness setting.
        /// </summary>
        /// <param name="awareness">
        /// The DPI awareness value to set.
        /// </param>
        public static void SetDpiAwareness(ProcessDpiAwareness awareness)
        {
            // Avoid popular Access Denied exception.
            if (Current.DpiAwareness == awareness)
                return;

            UnsafeNativeMethods.SetProcessDpiAwareness(awareness)
                               .EnsureNoWin32Error();
        }

        public ProcessDpiAwareness DpiAwareness
        {
            get
            {
                ProcessDpiAwareness awareness;
                UnsafeNativeMethods.GetProcessDpiAwareness(this.Handle.Value, out awareness)
                                   .EnsureNoWin32Error();
                return awareness;
            }
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
