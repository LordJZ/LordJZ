using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LordJZ.WinAPI.Native;

namespace LordJZ.WinAPI
{
    public struct FileMapping : IDisposable
    {
        public readonly Handle Handle;

        public FileMapping(Handle handle)
        {
            this.Handle = handle;
        }

        /// <summary>
        /// Opens a named file mapping object.
        /// </summary>
        /// <param name="desiredAccess">
        /// The access to the file mapping object.
        /// This access is checked against any security descriptor
        /// on the target file mapping object.
        /// </param>
        /// <param name="inheritHandle">
        /// If this parameter is <c>true</c>, a process created
        /// by the CreateProcess function can inherit the handle;
        /// otherwise, the handle cannot be inherited.
        /// </param>
        /// <param name="name">
        /// The name of the file mapping object to be opened.
        /// </param>
        /// <returns>
        /// The file mapping object.
        /// </returns>
        public static FileMapping Open(FileMappingSecurity desiredAccess, bool inheritHandle, string name)
        {
            IntPtr h = UnsafeNativeMethods.OpenFileMapping(desiredAccess, inheritHandle, name);
            Win32Error.EnsureNoWin32Error(h != IntPtr.Zero);

            return new FileMapping(new Handle(h));
        }


        public void Dispose()
        {
            Handle handle = this.Handle;
            handle.Close();
        }
    }
}
