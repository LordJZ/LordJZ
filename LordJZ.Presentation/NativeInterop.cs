using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using LordJZ.WinAPI;

namespace LordJZ.Presentation
{
    public static class NativeInterop
    {
        /// <summary>
        /// Gets the native window of the specified <see cref="System.Windows.Media.Visual"/> object.
        /// </summary>
        /// <param name="visual">
        /// The <see cref="System.Windows.Media.Visual"/> object which native window is queried.
        /// </param>
        /// <returns>
        /// A <see cref="System.IntPtr"/> that represents the native window
        /// of the specified <see cref="System.Windows.Media.Visual"/> object;
        /// or <see cref="System.IntPtr.Zero"/> of visual is null.
        /// </returns>
        public static NativeWindow GetNativeWindow(this Visual visual)
        {
            Contract.Requires(visual != null);

            var source = PresentationSource.FromVisual(visual) as HwndSource;
            Contract.Assume(source != null);

            return new NativeWindow(new Handle(source.Handle));
        }

        public static NativePoint ToNativePoint(this Point point)
        {
            return new NativePoint((int)point.X, (int)point.Y);
        }

        public static Point ToPoint(this NativePoint point)
        {
            return new Point(point.X, point.Y);
        }

        public static NativeRect ToNativeRect(this Rect rect)
        {
            return new NativeRect((int)rect.Left, (int)rect.Top, (int)rect.Right, (int)rect.Bottom);
        }

        public static Rect ToRect(this NativeRect rect)
        {
            return new Rect(rect.Left, rect.Top, rect.Width, rect.Height);
        }
    }
}
