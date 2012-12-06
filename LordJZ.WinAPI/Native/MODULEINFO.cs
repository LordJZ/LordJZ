using System;
using System.Runtime.InteropServices;

namespace LordJZ.WinAPI.Native
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct MODULEINFO
    {
        public static readonly int Size = Marshal.SizeOf(typeof(MODULEINFO));

        public IntPtr lpBaseOfDll;
        public int SizeOfImage;
        public IntPtr EntryPoint;
    }
}
