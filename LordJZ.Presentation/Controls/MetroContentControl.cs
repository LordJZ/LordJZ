using System.Windows;
using System.Windows.Controls;

namespace LordJZ.Presentation.Controls
{
    /// <remarks>
    /// Originally from http://xamlcoder.com/blog/2010/11/04/creating-a-metro-ui-style-control/
    /// Mostly taken from MahApps.Metro.
    /// </remarks>
    public class MetroContentControl : ContentControl
    {
        static readonly DependencyProperty ReverseTransitionProperty =
            DependencyProperty.Register("ReverseTransition", typeof(bool), typeof(MetroContentControl),
                                        new FrameworkPropertyMetadata(BooleanBoxes.False));

        bool ReverseTransition
        {
            get { return (bool)GetValue(ReverseTransitionProperty); }
            set { SetValue(ReverseTransitionProperty, BooleanBoxes.Box(value)); }
        }

        public MetroContentControl()
        {
            DefaultStyleKey = typeof(MetroContentControl);

            Loaded += this.MetroContentControlLoaded;
            Unloaded += this.MetroContentControlUnloaded;

            IsVisibleChanged += this.MetroContentControlIsVisibleChanged;
        }

        void MetroContentControlIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!IsVisible)
                VisualStateManager.GoToState(this, this.ReverseTransition ? "AfterUnLoadedReverse" : "AfterUnLoaded", false);
            else
                VisualStateManager.GoToState(this, this.ReverseTransition ? "AfterLoadedReverse" : "AfterLoaded", true);
        }

        private void MetroContentControlUnloaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, this.ReverseTransition ? "AfterUnLoadedReverse" : "AfterUnLoaded", false);
        }

        private void MetroContentControlLoaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, this.ReverseTransition ? "AfterLoadedReverse" : "AfterLoaded", true);
        }

        public void Reload()
        {
            VisualStateManager.GoToState(this, "BeforeLoaded", true);
            VisualStateManager.GoToState(this, this.ReverseTransition ? "AfterLoadedReverse" : "AfterLoaded", true);
        }

        public void SwitchContent(object newContent)
        {
            this.SwitchContent(newContent, default(ContentSwitchMode));
        }

        public void SwitchContent(object newContent, ContentSwitchMode mode)
        {
            if (mode == ContentSwitchMode.Instant)
            {
                this.Content = newContent;
            }
            else
            {
                this.ReverseTransition = mode == ContentSwitchMode.Backward;

                this.BeginInit();
                try
                {
                    this.Content = newContent;
                    this.Reload();
                }
                finally
                {
                    this.EndInit();
                }
            }
        }
    }
}
