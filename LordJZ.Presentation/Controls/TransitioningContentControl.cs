// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using LordJZ.Presentation;

//namespace MahApps.Metro.Controls
namespace LordJZ.Presentation.Controls
{
    /// <remarks>
    /// Mostly taken from MahApps.Metro.
    /// </remarks>
    public class TransitioningContentControl : ContentControl
    {
        internal const string PresentationGroup = "PresentationStates";
        internal const string NormalState = "Normal";
        internal const string PreviousContentPresentationSitePartName = "PreviousContentPresentationSite";
        internal const string CurrentContentPresentationSitePartName = "CurrentContentPresentationSite";
        private ContentPresenter CurrentContentPresentationSite { get; set; }
        private ContentPresenter PreviousContentPresentationSite { get; set; }
        private bool _allowIsTransitioningWrite;
        private Storyboard _currentTransition;

        public event RoutedEventHandler TransitionCompleted;
        public const string DefaultTransitionState = "DefaultTransition";

        public static readonly DependencyProperty IsTransitioningProperty
            = DependencyProperty.Register("IsTransitioning", typeof(bool), typeof(TransitioningContentControl),
                                          new PropertyMetadata(OnIsTransitioningPropertyChanged));

        public static readonly DependencyProperty TransitionProperty
            = DependencyProperty.Register("Transition", typeof(string), typeof(TransitioningContentControl),
                                          new PropertyMetadata(DefaultTransitionState, OnTransitionPropertyChanged));

        public static readonly DependencyProperty RestartTransitionOnContentChangeProperty
            = DependencyProperty.Register("RestartTransitionOnContentChange", typeof(bool),
                                          typeof(TransitioningContentControl),
                                          new PropertyMetadata(BooleanBoxes.False, OnRestartTransitionOnContentChangePropertyChanged));

        public bool IsTransitioning
        {
            get { return (bool)GetValue(IsTransitioningProperty); }
            private set
            {
                this._allowIsTransitioningWrite = true;
                SetValue(IsTransitioningProperty, BooleanBoxes.Box(value));
                this._allowIsTransitioningWrite = false;
            }
        }

        public string Transition
        {
            get { return GetValue(TransitionProperty) as string; }
            set { SetValue(TransitionProperty, value); }
        }

        public bool RestartTransitionOnContentChange
        {
            get { return (bool)GetValue(RestartTransitionOnContentChangeProperty); }
            set { SetValue(RestartTransitionOnContentChangeProperty, BooleanBoxes.Box(value)); }
        }

        private static void OnIsTransitioningPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = (TransitioningContentControl)d;

            if (!source._allowIsTransitioningWrite)
            {
                source.IsTransitioning = (bool)e.OldValue;
                throw new InvalidOperationException();
            }
        }

        private Storyboard CurrentTransition
        {
            get { return this._currentTransition; }
            set
            {
                // decouple event
                if (this._currentTransition != null)
                {
                    this._currentTransition.Completed -= this.OnTransitionCompleted;
                }

                this._currentTransition = value;

                if (this._currentTransition != null)
                {
                    this._currentTransition.Completed += this.OnTransitionCompleted;
                }
            }
        }

        private static void OnTransitionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = (TransitioningContentControl)d;
            var oldTransition = e.OldValue as string;
            var newTransition = e.NewValue as string;

            if (source.IsTransitioning)
            {
                source.AbortTransition();
            }

            // find new transition
            Storyboard newStoryboard = source.GetStoryboard(newTransition);

            // unable to find the transition.
            if (newStoryboard == null)
            {
                // could be during initialization of xaml that presentationgroups was not yet defined
                if (VisualStates.TryGetVisualStateGroup(source, PresentationGroup) == null)
                {
                    // will delay check
                    source.CurrentTransition = null;
                }
                else
                {
                    // revert to old value
                    source.SetValue(TransitionProperty, oldTransition);

                    throw new ArgumentException(
                        string.Format(CultureInfo.CurrentCulture, "Temporary removed exception message", newTransition));
                }
            }
            else
            {
                source.CurrentTransition = newStoryboard;
            }
        }

        private static void OnRestartTransitionOnContentChangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TransitioningContentControl)d).OnRestartTransitionOnContentChangeChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        protected virtual void OnRestartTransitionOnContentChangeChanged(bool oldValue, bool newValue)
        {
        }

        public TransitioningContentControl()
        {
            DefaultStyleKey = typeof(TransitioningContentControl);
        }

        public override void OnApplyTemplate()
        {
            if (this.IsTransitioning)
            {
                this.AbortTransition();
            }

            base.OnApplyTemplate();

            this.PreviousContentPresentationSite = GetTemplateChild(PreviousContentPresentationSitePartName) as ContentPresenter;
            this.CurrentContentPresentationSite = GetTemplateChild(CurrentContentPresentationSitePartName) as ContentPresenter;

            if (this.CurrentContentPresentationSite != null)
            {
                this.CurrentContentPresentationSite.Content = Content;
            }

            // hookup currenttransition
            Storyboard transition = this.GetStoryboard(this.Transition);
            this.CurrentTransition = transition;
            if (transition == null)
            {
                string invalidTransition = this.Transition;
                // revert to default
                this.Transition = DefaultTransitionState;

                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, "Temporary removed exception message", invalidTransition));
            }
            VisualStateManager.GoToState(this, NormalState, false);
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            this.StartTransition(oldContent, newContent);
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "newContent", Justification = "Should be used in the future.")]
        private void StartTransition(object oldContent, object newContent)
        {
            // both presenters must be available, otherwise a transition is useless.
            if (this.CurrentContentPresentationSite != null && this.PreviousContentPresentationSite != null)
            {
                this.CurrentContentPresentationSite.Content = newContent;

                this.PreviousContentPresentationSite.Content = oldContent;

                // and start a new transition
                if (!this.IsTransitioning || this.RestartTransitionOnContentChange)
                {
                    this.IsTransitioning = true;
                    VisualStateManager.GoToState(this, NormalState, false);
                    VisualStateManager.GoToState(this, this.Transition, true);
                }
            }
        }

        private void OnTransitionCompleted(object sender, EventArgs e)
        {
            this.AbortTransition();

            RoutedEventHandler handler = this.TransitionCompleted;
            if (handler != null)
            {
                handler(this, new RoutedEventArgs());
            }
        }

        public void AbortTransition()
        {
            // go to normal state and release our hold on the old content.
            VisualStateManager.GoToState(this, NormalState, false);
            this.IsTransitioning = false;
            if (this.PreviousContentPresentationSite != null)
            {
                this.PreviousContentPresentationSite.Content = null;
            }
        }

        private Storyboard GetStoryboard(string newTransition)
        {
            VisualStateGroup presentationGroup = VisualStates.TryGetVisualStateGroup(this, PresentationGroup);
            Storyboard newStoryboard = null;
            if (presentationGroup != null)
            {
                newStoryboard = presentationGroup.States
                    .OfType<VisualState>()
                    .Where(state => state.Name == newTransition)
                    .Select(state => state.Storyboard)
                    .FirstOrDefault();
            }
            return newStoryboard;
        }
    }
}