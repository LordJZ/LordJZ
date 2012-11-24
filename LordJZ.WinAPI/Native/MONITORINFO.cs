using System.Runtime.InteropServices;

namespace LordJZ.WinAPI.Native
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal class MONITORINFO
    {
        public static int Size = Marshal.SizeOf(typeof(MONITORINFO));

        public int cbSize = Size;
        public RECT rcMonitor = new RECT();
        public RECT rcWork = new RECT();
        public int dwFlags = 0;
    }
}
