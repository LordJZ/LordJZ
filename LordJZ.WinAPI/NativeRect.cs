using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LordJZ.WinAPI.Native;

namespace LordJZ.WinAPI
{
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct NativeRect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public NativeRect(int left, int top, int right, int bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        public int Width
        {
            get { return Math.Abs(this.Right - this.Left); }  // Abs needed for BIDI OS
        }

        public int Height
        {
            get { return this.Bottom - this.Top; }
        }

        #region Monitor

        public MonitorInfo Monitor
        {
            get { return this.GetMonitor(MonitorFallbackKind.Null); }
        }

        public MonitorInfo GetMonitor(MonitorFallbackKind fallback)
        {
            Contract.Requires(fallback == MonitorFallbackKind.Nearest ||
                              fallback == MonitorFallbackKind.Null ||
                              fallback == MonitorFallbackKind.Primary);

            IntPtr hMonitor = UnsafeNativeMethods.MonitorFromRect(ref this, (int)fallback);
            if (hMonitor == IntPtr.Zero)
                return null;

            MonitorInfo info = new MonitorInfo();
            UnsafeNativeMethods.GetMonitorInfo(hMonitor, info)
                               .EnsureNoWin32Error();
            return info;
        }

        #endregion

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                                 "[Left={0},Right={1},Top={2},Buttom={3}]",
                                 Left, Right, Top, Bottom);
        }
    }
}
