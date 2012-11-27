using System.Runtime.InteropServices;

namespace LordJZ.WinAPI
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MonitorInfo
    {
        #region Fields

        static readonly int Size = Marshal.SizeOf(typeof(MonitorInfo));

        internal int cbSize = Size;
        public NativeRect MonitorArea;
        public NativeRect WorkArea;
        public MonitorFlags Flags;

        #endregion

        public bool IsPrimary { get { return (this.Flags & MonitorFlags.Primary) == MonitorFlags.Primary; } }
    }
}
