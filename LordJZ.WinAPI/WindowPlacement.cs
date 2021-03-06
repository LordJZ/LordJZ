using System.Runtime.InteropServices;

namespace LordJZ.WinAPI
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class WindowPlacement
    {
        static readonly int Size = Marshal.SizeOf(typeof(WindowPlacement));

        internal int length = Size;
        int flags;
        int showCmd;
        public NativePoint MinPosition { get; set; }
        public NativePoint MaxPosition { get; set; }
        public NativeRect NormalPosition { get; set; }

        public WindowShow Show
        {
            get { return (WindowShow)showCmd; }
            set { showCmd = (int)value; }
        }

        public WindowPlacementFlags Flags
        {
            get { return (WindowPlacementFlags)flags; }
            set { flags = (int)value; }
        }
    }
}
