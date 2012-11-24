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
        public static readonly DependencyProperty ReverseTransitionProperty = DependencyProperty.Register("ReverseTransition", typeof(bool), typeof(MetroContentControl), new FrameworkPropertyMetadata(false));

        public bool ReverseTransition
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
    }
}
