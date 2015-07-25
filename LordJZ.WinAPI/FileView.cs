using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LordJZ.WinAPI.Native;

namespace LordJZ.WinAPI
{
    public struct FileView : IDisposable
    {
        public readonly IntPtr Location;

        public FileView(IntPtr location)
        {
            this.Location = location;
        }

        /// <summary>
        /// Maps a view of a file mapping into the address space of a calling process.
        /// </summary>
        /// <param name="mapping">
        /// A file mapping object.
        /// </param>
        /// <param name="desiredAccess">
        /// The type of access to a file mapping object, which determines the protection of the pages.
        /// </param>
        /// <param name="offset">
        /// The file offset where the view is to begin.
        /// </param>
        /// <param name="length">
        /// The number of bytes of a file mapping to map to the view.
        /// </param>
        /// <returns>
        /// The view of file object.
        /// </returns>
        public static FileView Create(FileMapping mapping, FileMappingSecurity desiredAccess, long offset, long length)
        {
            IntPtr h = UnsafeNativeMethods.MapViewOfFile(
                mapping.Handle.Value, desiredAccess,
                unchecked((uint)(offset >> 32)),
                unchecked((uint)(offset & uint.MaxValue)),
                (UIntPtr)length);

            Win32Error.EnsureNoWin32Error(h != IntPtr.Zero);

            return new FileView(h);
        }

        public MemoryBasicInformation MemoryInformation
        {
            get { return Environment.Is64BitProcess ? GetInfo64() : GetInfo32(); }
        }

        MemoryBasicInformation GetInfo32()
        {
            MEMORY_BASIC_INFORMATION32 info;
            IntPtr ret = UnsafeNativeMethods.VirtualQuery(this.Location, out info, (IntPtr)MEMORY_BASIC_INFORMATION32.Size);
            Win32Error.EnsureNoWin32Error(ret != IntPtr.Zero);
            return new MemoryBasicInformation(ref info);
        }

        MemoryBasicInformation GetInfo64()
        {
            MEMORY_BASIC_INFORMATION64 info;
            IntPtr ret = UnsafeNativeMethods.VirtualQuery(this.Location, out info, (IntPtr)MEMORY_BASIC_INFORMATION64.Size);
            Win32Error.EnsureNoWin32Error(ret != IntPtr.Zero);
            return new MemoryBasicInformation(ref info);
        }

        public long Size
        {
            get { return MemoryInformation.RegionSize; }
        }

        public unsafe Stream CreateStream()
        {
            return new UnmanagedMemoryStream((byte*)Location.ToPointer(), Size);
        }

        public void Dispose()
        {
            UnsafeNativeMethods.UnmapViewOfFile(Location).EnsureNoWin32Error();
        }
    }
}
