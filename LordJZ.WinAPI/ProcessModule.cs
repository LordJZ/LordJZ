using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using LordJZ.WinAPI.Native;

namespace LordJZ.WinAPI
{
    public struct ProcessModule
    {
        #region Fields

        readonly Process m_process;
        readonly Handle m_handle;

        #endregion

        #region Constructor

        public ProcessModule(Handle hProcess, Handle hModule)
        {
            Contract.Requires(hProcess != Process.InvalidProcessHandle);
            // TODO

            m_process = new Process(hProcess);
            m_handle = hModule;
        }

        public ProcessModule(Process process, Handle hModule)
            : this(process.Handle, hModule)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="Process"/> of the current process module.
        /// </summary>
        public Process Process
        {
            get { return m_process; }
        }

        /// <summary>
        /// Gets the handle of the current process module.
        /// </summary>
        public Handle Handle
        {
            get { return m_handle; }
        }

        /// <summary>
        /// Gets the fully qualified path for the file containing the current process module.
        /// </summary>
        public string FileName
        {
            get
            {
                var builder = new StringBuilder(Constants.MaxPath);
                uint size = UnsafeNativeMethods.GetModuleFileNameEx(m_process.Handle.Value, m_handle.Value,
                                                                    builder, builder.Capacity);

                Win32Error.EnsureNoWin32Error(size > 0);

                string filename = builder.ToString();

                if (string.Compare(filename, "\\SystemRoot\\System32\\smss.exe", StringComparison.OrdinalIgnoreCase) == 0)
                    filename = Path.Combine(Environment.SystemDirectory, "smss.exe");

                if (filename.StartsWith("\\\\?\\", StringComparison.Ordinal))
                    filename = filename.Substring(4);

                return filename;
            }
        }

        /// <summary>
        /// Gets the base name of the current process module.
        /// </summary>
        public string Name
        {
            get
            {
                var builder = new StringBuilder(Constants.MaxPath);
                uint size = UnsafeNativeMethods.GetModuleBaseName(m_process.Handle.Value, m_handle.Value,
                                                                  builder, builder.Capacity);

                Win32Error.EnsureNoWin32Error(size > 0);

                return builder.ToString();
            }
        }

        /// <summary>
        /// Gets the load address of the module.
        /// </summary>
        public IntPtr BaseAddress
        {
            get
            {
                MODULEINFO info;
                GetInfo(out info);
                return info.lpBaseOfDll;
            }
        }

        /// <summary>
        /// The size of the linear space that the module occupies, in bytes.
        /// </summary>
        public int Size
        {
            get
            {
                MODULEINFO info;
                GetInfo(out info);
                return info.SizeOfImage;
            }
        }

        /// <summary>
        /// Gets the entry point of the module.
        /// </summary>
        public IntPtr EntryPoint
        {
            get
            {
                MODULEINFO info;
                GetInfo(out info);
                return info.EntryPoint;
            }
        }

        #endregion

        #region Exports

        public unsafe ProcessExport[] GetExports()
        {
            IntPtr baseAddress = this.BaseAddress;
            IMAGE_DOS_HEADER dosHeader = m_process.ReadStructure<IMAGE_DOS_HEADER>(baseAddress);
            if (!dosHeader.IsValid)
                throw new InvalidOperationException();

            IMAGE_NT_HEADERS ntHeaders = m_process.ReadStructure<IMAGE_NT_HEADERS>(baseAddress + dosHeader.e_lfanew);
            if (!ntHeaders.IsValid)
                throw new InvalidOperationException();

            IMAGE_DATA_DIRECTORY dataDir;
            if (!ntHeaders.Is64)
            {
                if (ntHeaders.OptionalHeader32.NumberOfRvaAndSizes == 0)
                    throw new InvalidOperationException();

                dataDir = ntHeaders.OptionalHeader32.ExportTable;
            }
            else
            {
                if (ntHeaders.OptionalHeader64.NumberOfRvaAndSizes == 0)
                    throw new InvalidOperationException();

                dataDir = ntHeaders.OptionalHeader64.ExportTable;
            }

            IMAGE_EXPORT_DIRECTORY dir = m_process.ReadStructure<IMAGE_EXPORT_DIRECTORY>(baseAddress + dataDir.VirtualAddress);
            int count = dir.NumberOfNames;
            if (count != dir.NumberOfFunctions)
                throw new InvalidOperationException();

            ProcessExport[] exports = new ProcessExport[count];

            byte[] ptrArray = new byte[count * 4];

            m_process.ReadMemory(baseAddress + dir.AddressOfFunctions, ptrArray, 0, ptrArray.Length);

            fixed (byte* ptrOfPtrArray = ptrArray)
            {
                int* funcRvaArray = (int*)ptrOfPtrArray;
                for (int i = 0; i < count; i++)
                {
                    exports[i].Address = baseAddress + funcRvaArray[i];

                    exports[i].Process = m_process;
                    exports[i].Module = this;
                }
            }

            m_process.ReadMemory(baseAddress + dir.AddressOfNames, ptrArray, 0, ptrArray.Length);

            fixed (byte* ptrOfPtrArray = ptrArray)
            {
                int* nameRvaArray = (int*)ptrOfPtrArray;
                for (int i = 0; i < count; i++)
                    exports[i].Name = m_process.ReadCString(baseAddress + nameRvaArray[i]);
            }

            m_process.ReadMemory(baseAddress + dir.AddressOfNameOrdinals, ptrArray, 0, count * 2);

            fixed (byte* ptrOfPtrArray = ptrArray)
            {
                short* ordinalRvaArray = (short*)ptrOfPtrArray;
                for (int i = 0; i < count; i++)
                    exports[i].Ordinal = ordinalRvaArray[i];
            }

            return exports;
        }

        #endregion

        #region Private details

        void GetInfo(out MODULEINFO info)
        {
            UnsafeNativeMethods.GetModuleInformation(m_process.Handle.Value, m_handle.Value,
                                                     out info, MODULEINFO.Size)
                               .EnsureNoWin32Error();
        }

        #endregion
    }
}
