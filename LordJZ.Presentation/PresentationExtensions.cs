using System.Windows.Threading;
using LordJZ.WinAPI;
using LordJZ.WinAPI.Native;
using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using IFormsWindow = System.Windows.Forms.IWin32Window;
using IPresentationWindow = System.Windows.Interop.IWin32Window;

namespace LordJZ.Presentation
{
    public static class PresentationExtensions
    {
        #region Getters

        /// <summary>
        /// Gets the native window handle of the specified <see cref="System.Windows.Media.Visual"/> object.
        /// </summary>
        /// <param name="visual">
        /// The <see cref="System.Windows.Media.Visual"/> object which native wndow handle is queried.
        /// </param>
        /// <returns>
        /// A <see cref="System.IntPtr"/> that represents the native window handle
        /// of the specified <see cref="System.Windows.Media.Visual"/> object;
        /// or <see cref="System.IntPtr.Zero"/> of visual is null.
        /// </returns>
        public static IntPtr GetWin32Handle(this Visual visual)
        {
            if (visual == null)
                return IntPtr.Zero;

            var source = PresentationSource.FromVisual(visual) as HwndSource;
            if (source == null)
                return IntPtr.Zero;

            return source.Handle;
        }

        public static HwndSource GetHwndSource(this Visual visual)
        {
            Contract.Requires(visual != null);

            return PresentationSource.FromVisual(visual) as HwndSource;
        }

        public static Window GetWindow(this DependencyObject obj)
        {
            Contract.Requires(obj != null);

            //var dispatcher = obj.Dispatcher;
            //if (dispatcher.CheckAccess())
            return Window.GetWindow(obj);

            //return dispatcher.Invoke(() => Window.GetWindow(obj));
        }

        #endregion

        #region Thread-safeness providers

        public static object SafeGetValue(this DependencyObject obj, DependencyProperty prop)
        {
            Contract.Requires(obj != null);
            Contract.Requires(prop != null);

            var dispatcher = obj.Dispatcher;
            if (dispatcher.CheckAccess())
                return obj.GetValue(prop);

            return dispatcher.Invoke(() => obj.GetValue(prop));
        }

        public static void SafeSetValue(this DependencyObject obj, DependencyProperty prop, object value)
        {
            Contract.Requires(obj != null);
            Contract.Requires(prop != null);

            var dispatcher = obj.Dispatcher;
            if (dispatcher.CheckAccess())
                obj.SetValue(prop, value);
            else
                dispatcher.InvokeAsync(() => obj.SetValue(prop, value));
        }

        public static void SafeSetValue(this DependencyObject obj, DependencyProperty prop, bool value)
        {
            Contract.Requires(obj != null);
            Contract.Requires(prop != null);

            obj.SafeSetValue(prop, BooleanBoxes.Box(value));
        }

        public static void ThreadSafeAsync<T>(this T obj, Action<T> action) where T : DispatcherObject
        {
            Contract.Requires(obj != null);
            Contract.Requires(action != null);

            var dispatcher = obj.Dispatcher;
            if (dispatcher.CheckAccess())
                action(obj);
            else
                dispatcher.BeginInvoke(DispatcherPriority.Normal, action, obj);
        }

        public static TResult ThreadSafe<T, TResult>(this T obj, Func<T, TResult> func) where T : DispatcherObject
        {
            Contract.Requires(obj != null);
            Contract.Requires(func != null);

            var dispatcher = obj.Dispatcher;
            if (dispatcher.CheckAccess())
                return func(obj);

            return dispatcher.Invoke(() => func(obj));
        }

        #endregion

        #region WinForms HWND access interface

        public static IFormsWindow GetFormsWindowInterface(this Visual visual)
        {
            Contract.Requires(visual != null, "visual");

            return new FormsWindowWrapper(GetWin32Handle(visual));
        }

        class FormsWindowWrapper : IFormsWindow
        {
            readonly IntPtr m_handle;

            internal FormsWindowWrapper(IntPtr handle)
            {
                Contract.Requires(handle != IntPtr.Zero, "handle");

                m_handle = handle;
            }

            IntPtr IFormsWindow.Handle
            {
                get { return m_handle; }
            }
        }

        #endregion

        #region WPF HWND access interface

        public static IPresentationWindow GetPresentationWindowInterface(this Visual visual)
        {
            Contract.Requires(visual != null, "visual");

            return new PresentationWindowWrapper(GetWin32Handle(visual));
        }

        class PresentationWindowWrapper : IPresentationWindow
        {
            readonly IntPtr m_handle;

            internal PresentationWindowWrapper(IntPtr handle)
            {
                Contract.Requires(handle != IntPtr.Zero, "handle");

                m_handle = handle;
            }

            IntPtr IPresentationWindow.Handle
            {
                get { return m_handle; }
            }
        }

        #endregion

        #region ExtendFrameIntoClientArea

        public static void ExtendFrameIntoClientArea(this Window window, Thickness margins)
        {
            Contract.Requires(window != null);
            Contract.Requires(DesktopWindowManager.IsSupported);

            var source = window.GetHwndSource();
            if (source != null)
                InternalExtendFrameIntoClientArea(source.Handle, margins);
            else
                window.SourceInitialized += new ExtendFrameIntoClientArea_Closure(window, margins).Method;
        }

        class ExtendFrameIntoClientArea_Closure
        {
            readonly Window m_window;
            readonly Thickness m_thickness;

            public ExtendFrameIntoClientArea_Closure(Window window, Thickness thickness)
            {
                m_window = window;
                m_thickness = thickness;
            }

            public void Method(object o, EventArgs e)
            {
                InternalExtendFrameIntoClientArea(m_window.GetWin32Handle(), m_thickness);
                m_window.SourceInitialized -= this.Method;
            }
        }

        static void InternalExtendFrameIntoClientArea(IntPtr handle, Thickness thickness)
        {
            MARGINS margins;
            margins.bottomHeight = (int)thickness.Bottom;
            margins.leftWidth = (int)thickness.Left;
            margins.rightWidth = (int)thickness.Right;
            margins.topHeight = (int)thickness.Top;

            int result = UnsafeNativeMethods.DwmExtendFrameIntoClientArea(handle, ref margins);

            if (!Win32Error.IsSuccess(result))
                throw new Win32Exception(result);
        }

        #endregion

        #region Window Attributes

        internal static bool IsNonClientRenderingEnabled(this Window window)
        {
            Contract.Requires(window != null);

            var handle = window.GetWin32Handle();
            const int attr = (int)WindowAttribute.NonClientRenderingEnabled;
            int value;
            const int size = sizeof(int);

            var result = UnsafeNativeMethods.DwmGetWindowAttribute(handle, attr, out value, size);
            Win32Error.EnsureSuccess(result);

            return value != 0;
        }

        internal static NonClientRenderingPolicy GetNonClientRenderingPolicy(this Window window)
        {
            Contract.Requires(window != null);

            var handle = window.GetWin32Handle();
            const int attr = (int)WindowAttribute.NonClientRenderingPolicy;
            int value;
            const int size = sizeof(int);

            var result = UnsafeNativeMethods.DwmGetWindowAttribute(handle, attr, out value, size);
            Win32Error.EnsureSuccess(result);

            return (NonClientRenderingPolicy)value;
        }

        internal static void SetNonClientRenderingPolicy(this Window window, NonClientRenderingPolicy policy)
        {
            Contract.Requires(window != null);
            //Contract.Requires(Enum.IsDefined(typeof(NonClientRenderingPolicy), policy));

            var handle = window.GetWin32Handle();
            const int attr = (int)WindowAttribute.NonClientRenderingPolicy;
            var value = (int)policy;
            const int size = sizeof(int);

            var result = UnsafeNativeMethods.DwmSetWindowAttribute(handle, attr, ref value, size);
            Win32Error.EnsureSuccess(result);
        }

        #endregion

        internal static void SetDefaultBackgroundColor(this Window window, Color color)
        {
            var rgb = color.R | (color.G << 8) | (color.B << 16);

            var hBrushOld = SetClassLong(window.GetWin32Handle(), Constants.GCLP_HBRBACKGROUND,
                                         UnsafeNativeMethods.CreateSolidBrush(rgb));

            if (hBrushOld != IntPtr.Zero)
                UnsafeNativeMethods.DeleteObject(hBrushOld);
        }

        internal static IntPtr SetClassLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (Environment.Is64BitProcess)
                return UnsafeNativeMethods.SetClassLongPtr64(hWnd, nIndex, dwNewLong);

            return new IntPtr(UnsafeNativeMethods.SetClassLongPtr32(hWnd, nIndex, unchecked((uint)dwNewLong.ToInt32())));
        }

        public static void AddHook(this Window window, HwndSourceHook hook)
        {
            Contract.Requires(window != null);
            Contract.Requires(hook != null);

            var source = window.GetHwndSource();
            Contract.Assert(source != null);
            source.AddHook(hook);
        }
    }
}
