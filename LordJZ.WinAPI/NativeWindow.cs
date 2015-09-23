using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LordJZ.WinAPI.Native;

namespace LordJZ.WinAPI
{
    public struct NativeWindow
    {
        #region Statics

        public static NativeWindow[] Enumerate()
        {
            List<NativeWindow> list = new List<NativeWindow>();

            UnsafeNativeMethods.EnumWindows((hwnd, opaque) =>
                                            {
                                                list.Add(new NativeWindow(new Handle(hwnd)));
                                                return true;
                                            },
                                            IntPtr.Zero)
                               .EnsureNoWin32Error();

            return list.ToArray();
        }

        public static NativeWindow Active
        {
            get { return new NativeWindow(new Handle(UnsafeNativeMethods.GetActiveWindow())); }
        }

        public static NativeWindow Foreground
        {
            get { return new NativeWindow(new Handle(UnsafeNativeMethods.GetForegroundWindow())); }
        }

        #endregion

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

        public bool IsValid
        {
            get { return UnsafeNativeMethods.IsWindow(this.Handle.Value); }
        }

        public void GetThreadProcessId(out int threadId, out int processId)
        {
            threadId = UnsafeNativeMethods.GetWindowThreadProcessId(this.Handle.Value, out processId);
            Win32Error.EnsureNoWin32Error(threadId != 0);
            Win32Error.EnsureNoWin32Error(processId != 0);
        }

        public int ThreadId
        {
            get
            {
                int tid, pid;
                GetThreadProcessId(out tid, out pid);
                return tid;
            }
        }

        public int ProcessId
        {
            get
            {
                int tid, pid;
                GetThreadProcessId(out tid, out pid);
                return pid;
            }
        }

        public string Text
        {
            get
            {
                const SendMessageTimeoutFlags flags =
                    SendMessageTimeoutFlags.SMTO_NOTIMEOUTIFNOTHUNG |
                    SendMessageTimeoutFlags.SMTO_ABORTIFHUNG |
                    SendMessageTimeoutFlags.SMTO_ERRORONEXIT;

                IntPtr length;

                IntPtr result = UnsafeNativeMethods.SendMessageTimeout(
                    new HandleRef(this, this.Handle.Value),
                    Constants.WM_GETTEXTLENGTH,
                    IntPtr.Zero, IntPtr.Zero,
                    flags, 150, out length);

                Win32Error.EnsureNoWin32Error(result != IntPtr.Zero);

                StringBuilder sb = new StringBuilder((int)length + 1);
                result = UnsafeNativeMethods.SendMessageTimeout(
                    new HandleRef(this, this.Handle.Value),
                    Constants.WM_GETTEXT,
                    (IntPtr)sb.Capacity, sb,
                    flags, 150, out length);

                Win32Error.EnsureNoWin32Error(result != IntPtr.Zero);

                return sb.ToString();
            }
            set
            {
                const SendMessageTimeoutFlags flags =
                    SendMessageTimeoutFlags.SMTO_NOTIMEOUTIFNOTHUNG |
                    SendMessageTimeoutFlags.SMTO_ABORTIFHUNG |
                    SendMessageTimeoutFlags.SMTO_ERRORONEXIT;

                IntPtr length;
                IntPtr result = UnsafeNativeMethods.SendMessageTimeout(new HandleRef(this, this.Handle.Value), Constants.WM_SETTEXT, IntPtr.Zero, value, flags, 150, out length);

                Win32Error.EnsureNoWin32Error(result != IntPtr.Zero);
            }
        }

        public string ClassName
        {
            get
            {
                int length = 64;
                while (true)
                {
                    StringBuilder sb = new StringBuilder(length);

                    int written = UnsafeNativeMethods.GetClassName(this.Handle.Value, sb, sb.Capacity);

                    Win32Error.EnsureNoWin32Error(written != 0);

                    if (sb.Length != length - 1)
                        return sb.ToString();

                    length += length >> 1;
                }
            }
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

	    public bool Zoomed
	    {
		    get { return UnsafeNativeMethods.IsZoomed(this.Handle.Value); }
	    }

        public Handle SystemMenu
        {
            get { return new Handle(UnsafeNativeMethods.GetSystemMenu(this.Handle.Value, false)); }
        }

        #endregion
    }
}
