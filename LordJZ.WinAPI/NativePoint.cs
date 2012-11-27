using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using LordJZ.WinAPI.Native;

namespace LordJZ.WinAPI
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NativePoint
    {
        int m_x;
        int m_y;

        public NativePoint(int x, int y)
        {
            m_x = x;
            m_y = y;
        }

        public int X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        public int Y
        {
            get { return m_y; }
            set { m_y = value; }
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

            IntPtr hMonitor = UnsafeNativeMethods.MonitorFromPoint(this, (int)fallback);
            if (hMonitor == IntPtr.Zero)
                return null;

            MonitorInfo info = new MonitorInfo();

            UnsafeNativeMethods.GetMonitorInfo(hMonitor, info)
                               .EnsureNoWin32Error();

            return info;
        }

        #endregion

    }
}
