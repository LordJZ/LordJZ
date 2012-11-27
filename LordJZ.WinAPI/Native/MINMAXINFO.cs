using System.Runtime.InteropServices;

namespace LordJZ.WinAPI.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MINMAXINFO
    {
        public NativePoint ptReserved;
        public NativePoint ptMaxSize;
        public NativePoint ptMaxPosition;
        public NativePoint ptMinTrackSize;
        public NativePoint ptMaxTrackSize;
    }
}