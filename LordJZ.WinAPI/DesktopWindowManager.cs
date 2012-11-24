
using System;
using System.Diagnostics.Contracts;
using LordJZ.WinAPI.Native;

namespace LordJZ.WinAPI
{
    /// <summary>
    /// Serves as a managed wrapper around the native Desktop Window Manager (DWM) API.
    /// </summary>
    public static class DesktopWindowManager
    {
        /// <summary>
        /// Gets the value indicating whether the Desktop Window Manager (DWM)
        /// is supported in the current environment.
        /// </summary>
        public static bool IsSupported
        {
            get { return Environment.OSVersion.Version.Major >= 6; }
        }

        /// <summary>
        /// Gets the value indicating whether the Desktop Window Manager (DWM) composition is enabled.
        /// </summary>
        /// <remarks>
        /// Since Windows 8, the Desktop Window Manager (DWM) composition is always enabled.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// The Desktop Wondow Manager (DWM) is not supported in the current environment.
        /// </exception>
        public static bool IsCompositionEnabled
        {
            get
            {
                CheckSupported();

                return UnsafeNativeMethods.DwmIsCompositionEnabled();
            }
        }

        //public static void SetWindowAttribute(IntPtr handle, DwmWindowAttribute attribute, int value)
        //{
        //    switch (attribute)
        //    {
        //        case DwmWindowAttribute.NonClientRenderingEnabled:
        //            break;
        //        case DwmWindowAttribute.NonClientRenderingPolicy:
        //            break;
        //        case DwmWindowAttribute.TransitionsForceDisabled:
        //            break;
        //        case DwmWindowAttribute.AllowNonClientPaint:
        //            break;
        //        case DwmWindowAttribute.CaptionButtonBounds:
        //            break;
        //        case DwmWindowAttribute.NonClientRtlLayout:
        //            break;
        //        case DwmWindowAttribute.ForceIconicRepresentation:
        //            break;
        //        case DwmWindowAttribute.Flip3DPolicy:
        //            break;
        //        case DwmWindowAttribute.ExtendedFrameBounds:
        //            break;
        //        case DwmWindowAttribute.HasIconicBitmap:
        //            break;
        //        case DwmWindowAttribute.DisallowPeek:
        //            break;
        //        case DwmWindowAttribute.ExcludedFromPeek:
        //            break;
        //        case DwmWindowAttribute.Cloak:
        //            break;
        //        case DwmWindowAttribute.Cloaked:
        //            break;
        //        case DwmWindowAttribute.FreezeRepresentation:
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException("attribute");
        //    }
        //}

        #region Private

        const string s_dwmNotSupportedMessage =
            "The Desktop Window Manager (DWM) is not supported in the current environment.";

        [ContractAbbreviator]
        static void CheckSupported()
        {
            Contract.Requires(IsSupported, s_dwmNotSupportedMessage);
        }

        #endregion
    }
}
