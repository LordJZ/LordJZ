using System.Windows;
using System.Windows.Controls;
using LordJZ.Presentation.Controls;

namespace LordJZ.Presentation.Controls
{
    /// <summary>
    /// Represents a <see cref="Button"/> that contains an <see cref="Icon"/>.
    /// </summary>
    public class IconButton : Button
    {
        #region PredefinedIcon

        public Icons PredefinedIcon
        {
            get { return (Icons)GetValue(PredefinedIconProperty); }
            set { SetValue(PredefinedIconProperty, value); }
        }

        public static readonly DependencyProperty PredefinedIconProperty =
            IconProperties.PredefinedIconProperty.AddOwner(typeof(IconButton));

        #endregion

        #region IconName

        public string IconName
        {
            get { return (string)GetValue(IconNameProperty); }
            set { SetValue(IconNameProperty, value); }
        }

        public static readonly DependencyProperty IconNameProperty =
            IconProperties.IconNameProperty.AddOwner(typeof(IconButton));

        #endregion

        #region IconSize

        public IconSizes IconSize
        {
            get { return (IconSizes)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        public static readonly DependencyProperty IconSizeProperty =
            IconProperties.IconSizeProperty.AddOwner(typeof(IconButton));

        #endregion

        #region ShowCircle

        public bool ShowCircle
        {
            get { return (bool)GetValue(ShowCircleProperty); }
            set { SetValue(ShowCircleProperty, BooleanBoxes.Box(value)); }
        }

        public static readonly DependencyProperty ShowCircleProperty =
            IconProperties.ShowCircleProperty.AddOwner(typeof(IconButton));

        #endregion

        #region ShowBackground

        public bool ShowBackground
        {
            get { return (bool)GetValue(ShowBackgroundProperty); }
            set { SetValue(ShowBackgroundProperty, BooleanBoxes.Box(value)); }
        }

        public static readonly DependencyProperty ShowBackgroundProperty =
            IconProperties.ShowBackgroundProperty.AddOwner(typeof(IconButton));

        #endregion

        #region BackgroundOpacity

        public double BackgroundOpacity
        {
            get { return (double)GetValue(BackgroundOpacityProperty); }
            set { SetValue(BackgroundOpacityProperty, value); }
        }

        public static readonly DependencyProperty BackgroundOpacityProperty =
            IconProperties.BackgroundOpacityProperty.AddOwner(typeof(IconButton));

        #endregion
    }
}
