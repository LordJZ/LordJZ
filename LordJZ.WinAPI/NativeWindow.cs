using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LordJZ.WinAPI.Native;

namespace LordJZ.WinAPI
{
    public struct NativeWindow
    {
        #region Fields

        readonly Handle m_handle;

        #endregion

        #region Constructor

        public NativeWindow(Handle handle)
        {
            m_handle = handle;
        }

        #endregion

        #region Properties

        public Handle Handle
        {
            get { return m_handle; }
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

            IntPtr hMonitor = UnsafeNativeMethods.MonitorFromWindow(this.Handle.Value, (int)fallback);
            if (hMonitor == IntPtr.Zero)
                return null;

            MonitorInfo info = new MonitorInfo();
            UnsafeNativeMethods.GetMonitorInfo(hMonitor, info)
                               .EnsureNoWin32Error();
            return info;
        }

        public NativePoint MonitorDpi
        {
            get
            {
                IntPtr hMonitor = UnsafeNativeMethods.MonitorFromWindow(this.Handle.Value,
                                                                        (int)MonitorFallbackKind.Nearest);

                uint x, y;
                UnsafeNativeMethods.GetDpiForMonitor(hMonitor, 0, out x, out y)
                                   .EnsureNoWin32Error();

                return new NativePoint(checked((int)x), checked((int)y));
            }
        }

        #endregion

        public WindowPlacement Placement
        {
            get
            {
                WindowPlacement placement = new WindowPlacement();
                UnsafeNativeMethods.GetWindowPlacement(this.Handle.Value, placement)
                                   .EnsureNoWin32Error();
                return placement;
            }
            set
            {
                Contract.Requires(value != null);

                UnsafeNativeMethods.SetWindowPlacement(this.Handle.Value, value)
                                   .EnsureNoWin32Error();
            }
        }

        public Handle SystemMenu
        {
            get { return new Handle(UnsafeNativeMethods.GetSystemMenu(this.Handle.Value, false)); }
        }

        #endregion
    }
}
