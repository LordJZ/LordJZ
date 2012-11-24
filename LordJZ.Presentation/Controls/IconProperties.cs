using System.Diagnostics.Contracts;
using System.Windows;

namespace LordJZ.Presentation.Controls
{
    public static class IconProperties
    {
        #region IconSize

        public static IconSizes GetIconSize(DependencyObject o)
        {
            Contract.Requires(o != null);

            return (IconSizes)o.GetValue(IconSizeProperty);
        }

        public static void SetIconSize(DependencyObject o, IconSizes value)
        {
            Contract.Requires(o != null);

            o.SetValue(IconSizeProperty, value);
        }

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.RegisterAttached(
                "IconSize", typeof(IconSizes), typeof(IconProperties),
                new FrameworkPropertyMetadata(IconSizes.Big,
                                              FrameworkPropertyMetadataOptions.Inherits));

        #endregion

        #region ShowCircle

        public static bool GetShowCircle(DependencyObject o)
		{
            Contract.Requires(o != null);

            return (bool)o.GetValue(ShowCircleProperty);
		}

		public static void SetShowCircle(DependencyObject o, bool value)
		{
            Contract.Requires(o != null);

            o.SetValue(ShowCircleProperty, BooleanBoxes.Box(value));
		}

        public static readonly DependencyProperty ShowCircleProperty =
            DependencyProperty.RegisterAttached(
                "ShowCircle", typeof(bool), typeof(IconProperties),
                new FrameworkPropertyMetadata(BooleanBoxes.True,
                                              FrameworkPropertyMetadataOptions.Inherits));

        #endregion

        #region ShowBackground

        public static bool GetShowBackground(DependencyObject o)
		{
            Contract.Requires(o != null);

            return (bool)o.GetValue(ShowBackgroundProperty);
		}

		public static void SetShowBackground(DependencyObject o, bool value)
		{
            Contract.Requires(o != null);

            o.SetValue(ShowBackgroundProperty, BooleanBoxes.Box(value));
		}

        public static readonly DependencyProperty ShowBackgroundProperty =
            DependencyProperty.RegisterAttached(
                "ShowBackground", typeof(bool), typeof(IconProperties),
                new FrameworkPropertyMetadata(BooleanBoxes.False,
                                              FrameworkPropertyMetadataOptions.Inherits));

        #endregion

        #region PredefinedIcon

        public const Icons DefaultIcon = Icons.None;

        public static Icons GetPredefinedIcon(DependencyObject o)
		{
            Contract.Requires(o != null);

            return (Icons)o.GetValue(PredefinedIconProperty);
		}

        public static void SetPredefinedIcon(DependencyObject o, Icons value)
		{
            Contract.Requires(o != null);

            o.SetValue(PredefinedIconProperty, value);
		}

        public static readonly DependencyProperty PredefinedIconProperty =
            DependencyProperty.RegisterAttached(
                "PredefinedIcon", typeof(Icons), typeof(IconProperties),
                new FrameworkPropertyMetadata(DefaultIcon,
                                              FrameworkPropertyMetadataOptions.Inherits));

        #endregion

        #region IconName

        public const string DefaultIconName = null;

        public static string GetIconName(DependencyObject o)
		{
            Contract.Requires(o != null);

            return (string)o.GetValue(IconNameProperty);
		}

		public static void SetIconName(DependencyObject o, string value)
		{
            Contract.Requires(o != null);

            o.SetValue(IconNameProperty, value);
		}

        public static readonly DependencyProperty IconNameProperty =
            DependencyProperty.RegisterAttached(
                "IconName", typeof(string), typeof(IconProperties),
                new FrameworkPropertyMetadata(DefaultIconName,
                                              FrameworkPropertyMetadataOptions.Inherits));

        #endregion

        #region BackgroundOpacity

        public static double GetBackgroundOpacity(DependencyObject o)
		{
            Contract.Requires(o != null);

            return (double)o.GetValue(BackgroundOpacityProperty);
		}

		public static void SetBackgroundOpacity(DependencyObject o, double value)
		{
            Contract.Requires(o != null);

            o.SetValue(BackgroundOpacityProperty, value);
		}

        public static readonly DependencyProperty BackgroundOpacityProperty =
            DependencyProperty.RegisterAttached(
                "BackgroundOpacity", typeof(double), typeof(IconProperties),
                new FrameworkPropertyMetadata(1.0,
                                              FrameworkPropertyMetadataOptions.Inherits));

        #endregion
    }
}
