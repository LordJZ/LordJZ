using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LordJZ.Presentation.Controls;

namespace LordJZ.Presentation.Controls
{
    [TemplatePart(Name = PART_Grid, Type = typeof(FrameworkElement))]
    public class Icon : Control
    {
        const string PART_Grid = "PART_Grid";

        FrameworkElement m_container;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.m_container = this.GetTemplateChild(PART_Grid) as FrameworkElement;

            this.UpdateIcon();
        }

        #region IconSize

        public IconSizes IconSize
        {
            get { return (IconSizes)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        public static readonly DependencyProperty IconSizeProperty =
            IconProperties.IconSizeProperty.AddOwner(typeof(Icon),
                                                     new FrameworkPropertyMetadata(IconSizeChanged));

        static void IconSizeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var icon = o as Icon;
            if (icon != null)
                icon.UpdateIconSize();
        }

        #endregion

        #region ShowCircle

        public bool ShowCircle
        {
            get { return (bool)GetValue(ShowCircleProperty); }
            set { SetValue(ShowCircleProperty, BooleanBoxes.Box(value)); }
        }

        public static readonly DependencyProperty ShowCircleProperty =
            IconProperties.ShowCircleProperty.AddOwner(typeof(Icon));

        #endregion

        #region ShowBackground

        public bool ShowBackground
        {
            get { return (bool)GetValue(ShowBackgroundProperty); }
            set { SetValue(ShowBackgroundProperty, BooleanBoxes.Box(value)); }
        }

        public static readonly DependencyProperty ShowBackgroundProperty =
            IconProperties.ShowBackgroundProperty.AddOwner(typeof(Icon));

        #endregion

        #region PredefinedIcon

        public const Icons DefaultIcon = Icons.None;

        public Icons PredefinedIcon
        {
            get { return (Icons)GetValue(PredefinedIconProperty); }
            set { SetValue(PredefinedIconProperty, value); }
        }

        public static readonly DependencyProperty PredefinedIconProperty =
            IconProperties.PredefinedIconProperty.AddOwner(typeof(Icon),
                                                           new FrameworkPropertyMetadata(IconChanged));

        static void IconChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var icon = o as Icon;
            if (icon == null)
                return;

            if (icon.m_iconChanging)
                return;

            try
            {
                icon.m_iconChanging = true;
                icon.IconName = DefaultIconName;
                icon.UpdateIconContent();
            }
            finally
            {
                icon.m_iconChanging = false;
            }
        }

        #endregion

        #region IconName

        public const string DefaultIconName = null;

        public string IconName
        {
            get { return (string)GetValue(IconNameProperty); }
            set { SetValue(IconNameProperty, value); }
        }

        public static readonly DependencyProperty IconNameProperty =
            IconProperties.IconNameProperty.AddOwner(typeof(Icon),
                                                     new FrameworkPropertyMetadata(IconNameChanged));

        static void IconNameChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var icon = o as Icon;
            if (icon == null)
                return;

            if (icon.m_iconChanging)
                return;

            try
            {
                icon.m_iconChanging = true;
                icon.PredefinedIcon = DefaultIcon;
                icon.UpdateIconContent();
            }
            finally
            {
                icon.m_iconChanging = false;
            }
        }

        #endregion

        #region BackgroundOpacity

        public double BackgroundOpacity
        {
            get { return (double)GetValue(BackgroundOpacityProperty); }
            set { SetValue(BackgroundOpacityProperty, value); }
        }

        public static readonly DependencyProperty BackgroundOpacityProperty =
            IconProperties.BackgroundOpacityProperty.AddOwner(typeof(Icon));

        #endregion

        #region DisplayedIcon

        public Visual DisplayedIcon
        {
            get { return (Visual)GetValue(DisplayedIconProperty); }
            set { SetValue(DisplayedIconProperty, value); }
        }

        public static readonly DependencyProperty DisplayedIconProperty =
            DependencyProperty.Register("DisplayedIcon", typeof(Visual), typeof(Icon));

        #endregion

        bool m_iconChanging;

        void UpdateIcon()
        {
            this.UpdateIconContent();
            this.UpdateIconSize();
        }

        void UpdateIconContent()
        {
            string key;

            do
            {
                var iconName = this.IconName;
                if (!string.IsNullOrWhiteSpace(iconName))
                {
                    key = iconName;
                    break;
                }

                var icon = this.PredefinedIcon;
                if (icon != DefaultIcon)
                {
                    key = icon.ToString();
                    break;
                }

                key = null;
            }
            while (false);

            if (key != null)
                this.SetResourceReference(DisplayedIconProperty, "Icon_" + key);
            else
                this.DisplayedIcon = null;
        }

        void UpdateIconSize()
        {
            var container = this.m_container;
            if (container == null)
                return;

            var size = this.IconSize;

            double measure;
            switch (size)
            {
                case IconSizes.Tiny:
                    measure = 16;
                    break;
                case IconSizes.Small:
                    measure = 24;
                    break;
                case IconSizes.Big:
                    measure = 48;
                    break;
                default:
                    return;
            }

            container.MaxWidth = measure;
            container.MaxHeight = measure;
        }
    }
}
