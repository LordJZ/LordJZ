using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace LordJZ.WinAPI.Native
{
    /// <devdoc>http://msdn.microsoft.com/en-us/library/ms182161.aspx</devdoc>
    [SuppressUnmanagedCodeSecurity]
    internal static class UnsafeNativeMethods
    {
        internal const string user32 = "user32";
        internal const string dwmapi = "dwmapi";
        internal const string kernel32 = "kernel32";
        internal const string gdi32 = "gdi32";
        internal const string ntdll = "ntdll";
        internal const string psapi = "psapi";
        internal const string shcore = "shcore"; // ShellScalingAPI.h

        #region dwmapi

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/aa969518%28v=vs.85%29.aspx</devdoc>
        [DllImport(dwmapi, PreserveSig = false, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DwmIsCompositionEnabled();

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/aa969512%28v=vs.85%29.aspx</devdoc>
        [DllImport(dwmapi, PreserveSig = true, CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Error)]
        internal static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, [In] ref MARGINS pMarInset);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/aa969524%28v=vs.85%29.aspx</devdoc>
        [DllImport(dwmapi, PreserveSig = true, CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
        internal static extern int DwmSetWindowAttribute([In] IntPtr hwnd, [In] int attr, [In] ref int attrValue, [In] int attrSize);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/aa969524%28v=vs.85%29.aspx</devdoc>
        [DllImport(dwmapi, PreserveSig = true, CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
        internal static extern int DwmGetWindowAttribute([In] IntPtr hwnd, [In] int attr, out int attrValue, [In] int attrSize);

        #endregion

        #region user32

        [DllImport(user32, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(
            HandleRef hWnd,
            int Msg,
            IntPtr wParam,
            [Out] StringBuilder lParam,
            SendMessageTimeoutFlags flags,
            uint timeout,
            out IntPtr result);

        [DllImport(user32, CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(HandleRef hWnd, int Msg, IntPtr wParam, [Out] StringBuilder lParam);

        [DllImport(user32, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(
            HandleRef hWnd,
            int Msg,
            IntPtr wParam,
            [MarshalAs(UnmanagedType.LPTStr), In] string lParam,
            SendMessageTimeoutFlags flags,
            uint timeout,
            out IntPtr result);

        [DllImport(user32, CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(HandleRef hWnd, int Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPTStr), In] string lParam);

        [DllImport(user32, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(
            HandleRef hWnd,
            int Msg,
            IntPtr wParam,
            IntPtr lParam,
            SendMessageTimeoutFlags flags,
            uint timeout,
            out IntPtr result);

        [DllImport(user32, CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(HandleRef hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/ms633572%28v=vs.85%29.aspx</devdoc>
        [DllImport(user32, CallingConvention = CallingConvention.Winapi)]
        internal static extern IntPtr DefWindowProc([In] IntPtr hwnd, [In] int msg, [In] IntPtr wParam, [In] IntPtr lParam);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/dd144901%28v=VS.85%29.aspx</devdoc>
        [DllImport(user32, EntryPoint = "GetMonitorInfoW", ExactSpelling = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetMonitorInfo([In] IntPtr hMonitor, [Out] MonitorInfo lpmi);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/dd145064%28v=VS.85%29.aspx</devdoc>
        [DllImport(user32)]
        internal static extern IntPtr MonitorFromWindow([In] IntPtr handle, [In] int flags);

        [DllImport(user32)]
        internal static extern IntPtr MonitorFromPoint([In] NativePoint point, [In] int flags);

        [DllImport(user32)]
        internal static extern IntPtr MonitorFromRect([In] ref NativeRect rect, [In] int flags);

        [DllImport(user32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        internal delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

        [DllImport(user32)]
        internal static extern IntPtr GetActiveWindow();

        [DllImport(user32)]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport(user32)]
        internal static extern int GetClassName(IntPtr hWnd, [Out] StringBuilder lpClassName, int nMaxCount);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/ms633545(v=vs.85).aspx</devdoc>
        [DllImport(user32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        
        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/ms647486%28v=vs.85%29.aspx</devdoc>
        [DllImport(user32, CharSet = CharSet.Unicode, ExactSpelling = true, EntryPoint = "LoadStringW", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        internal static extern int LoadString([In] [Optional] IntPtr hInstance, [In] uint uID, [Out] StringBuilder lpBuffer, [In] int nBufferMax);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/ms633528(v=vs.85).aspx</devdoc>
        [DllImport(user32, CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern bool IsWindow([In] [Optional] IntPtr hWnd);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/ms647985(v=vs.85).aspx</devdoc>
        [DllImport(user32)]
        internal static extern IntPtr GetSystemMenu([In] IntPtr hWnd, [In] bool bRevert);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/ms648003(v=vs.85).aspx</devdoc>
        [DllImport(user32)]
        internal static extern uint TrackPopupMenuEx([In] IntPtr hmenu, [In] uint fuFlags, [In] int x, [In] int y, [In] IntPtr hwnd, [In] [Optional] IntPtr lptpm);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/ms644944(v=vs.85).aspx</devdoc>
        [DllImport(user32, EntryPoint = "PostMessage", SetLastError = true)]
        private static extern bool _PostMessage([In] [Optional] IntPtr hWnd, [In] uint Msg, [In] IntPtr wParam, [In] IntPtr lParam);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/ms648390(v=vs.85).aspx</devdoc>
        [DllImport(user32)]
        internal static extern bool GetCursorPos([Out] out Win32Point pt);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/ms646258(v=vs.85).aspx</devdoc>
        [DllImport(user32, CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern int GetDoubleClickTime();

        [DllImport(user32, EntryPoint = "SetClassLong")]
        internal static extern uint SetClassLongPtr32(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport(user32, EntryPoint = "SetClassLongPtr")]
        internal static extern IntPtr SetClassLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport(user32)]
        internal static extern bool SetWindowPlacement(IntPtr hWnd, [In] WindowPlacement lpwndpl);

        [DllImport(user32)]
        internal static extern bool GetWindowPlacement(IntPtr hWnd, [Out] WindowPlacement lpwndpl);

        [DllImport(user32)]
        internal static extern bool IsZoomed(IntPtr hWnd);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/ms647636(v=vs.85).aspx</devdoc>
        [DllImport(user32)]
        internal static extern uint EnableMenuItem(IntPtr hMenu, uint itemId, uint uEnable);

        [DllImport(user32, SetLastError = true)]
        internal static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        #endregion

        #region kernel32

        /// <devdoc>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms684175%28v=vs.85%29.aspx
        /// </devdoc>
        [DllImport(kernel32, CharSet = CharSet.Unicode, ExactSpelling = true, EntryPoint = "LoadLibraryW",
            SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        internal static extern IntPtr LoadLibrary([In] [MarshalAs(UnmanagedType.LPWStr)] string lpFileName);

        /// <devdoc>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms683152%28v=vs.85%29.aspx
        /// </devdoc>
        [DllImport(kernel32, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FreeLibrary([In] IntPtr hModule);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern IntPtr OpenProcess(int dwDesiredAccess,
                                                  [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
                                                  int dwProcessId);

        [DllImport(kernel32)]
        internal static extern IntPtr GetCurrentProcess();

        [DllImport(kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr hObject);

        [DllImport(kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DuplicateHandle(
            IntPtr hSourceProcessHandle, IntPtr hSourceHandle,
            IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle,
            uint dwDesiredAccess,
            [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern bool SetHandleInformation(IntPtr hObject, uint dwMask, uint dwFlags);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern bool GetHandleInformation(IntPtr hObject, out uint lpdwFlags);

        [DllImport(kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetExitCodeProcess(IntPtr hProcess, out uint lpExitCode);

        [DllImport(kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern bool ReadProcessMemory(
            IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, IntPtr dwSize,
            out IntPtr lpNumberOfBytesRead);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern bool WriteProcessMemory(
            IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, IntPtr nSize,
            out IntPtr lpNumberOfBytesWritten);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern int GetProcessId(IntPtr hProcess);

        [DllImport(kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr processHandle,
                                                 [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process);

        [DllImport(kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetPriorityClass(IntPtr hProcess, ProcessPriority priorityClass);

        [DllImport(kernel32, SetLastError = true)]
        public static extern ProcessPriority GetPriorityClass(IntPtr hProcess);

        [DllImport(kernel32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr OpenFileMapping(
             FileMappingSecurity dwDesiredAccess,
             [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
             [MarshalAs(UnmanagedType.LPTStr)] string lpName);

        [DllImport(kernel32, SetLastError = true)]
        public static extern IntPtr MapViewOfFile(
            IntPtr hFileMappingObject,
            FileMappingSecurity dwDesiredAccess,
            UInt32 dwFileOffsetHigh,
            UInt32 dwFileOffsetLow,
            UIntPtr dwNumberOfBytesToMap);

        [DllImport(kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnmapViewOfFile(
            IntPtr lpBaseAddress);

        [DllImport(kernel32, SetLastError = true)]
        public static extern IntPtr VirtualQuery(
            IntPtr lpAddress,
            out MEMORY_BASIC_INFORMATION32 lpBuffer,
            IntPtr dwLength);

        [DllImport(kernel32, SetLastError = true)]
        public static extern IntPtr VirtualQuery(
            IntPtr lpAddress,
            out MEMORY_BASIC_INFORMATION64 lpBuffer,
            IntPtr dwLength);

        #endregion

        #region ntdll

        [DllImport(ntdll, SetLastError = true)]
        internal static extern int NtQueryInformationProcess(
            IntPtr processHandle, int processInformationClass,
            IntPtr processInformation, uint processInformationLength, IntPtr returnLength);

        #endregion

        #region psapi

        [DllImport(psapi, SetLastError = true)]
        internal static extern bool EnumProcessModules(IntPtr hProcess, IntPtr lphModule, int cb,
                                                       [MarshalAs(UnmanagedType.I4)] out int lpcbNeeded);

        [DllImport(psapi, SetLastError = true)]
        internal static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule,
                                                        [Out] StringBuilder lpBaseName,
                                                        int nSize);

        [DllImport(psapi, SetLastError = true)]
        internal static extern uint GetModuleBaseName(IntPtr hProcess, IntPtr hModule,
                                                      [Out] StringBuilder lpBaseName,
                                                      int nSize);

        [DllImport(psapi, SetLastError = true)]
        internal static extern bool GetModuleInformation(IntPtr hProcess, IntPtr hModule,
                                                         out MODULEINFO lpmodinfo, int cb);

        #endregion

        #region gdi32

        [DllImport(gdi32)]
        internal static extern IntPtr CreateSolidBrush(int crColor);

        [DllImport(gdi32)]
        internal static extern bool DeleteObject(IntPtr hObject);

        #endregion

        #region shcore

        [DllImport(shcore)]
        internal static extern int GetProcessDpiAwareness([Optional] IntPtr hProcess, out ProcessDpiAwareness awareness);

        [DllImport(shcore)]
        internal static extern int SetProcessDpiAwareness(ProcessDpiAwareness awareness);

        [DllImport(shcore)]
        internal static extern int GetDpiForMonitor([In] IntPtr hMonitor, [In] int dpiType,
                                                    [Out] out uint dpiX, [Out] out uint dpiY);

        #endregion

        internal static void PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam)
        {
            if (!_PostMessage(hWnd, Msg, wParam, lParam))
            {
                throw new Win32Exception();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public readonly Int32 X;
            public readonly Int32 Y;
        };

        internal static int GET_X_LPARAM(IntPtr lParam)
        {
            return LOWORD(lParam.ToInt32());
        }

        internal static int GET_Y_LPARAM(IntPtr lParam)
        {
            return HIWORD(lParam.ToInt32());
        }

        private static int HIWORD(int i)
        {
            return (short)(i >> 16);
        }

        private static int LOWORD(int i)
        {
            return (short)(i & 0xFFFF);
        }
    }
}