using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LordJZ.Presentation.Controls
{
    /// <remarks>
    /// Mostly taken from MahApps.Metro.
    /// </remarks>
    internal static class VisualStates
    {
        public const string GroupCommon = "CommonStates";
        public const string StateNormal = "Normal";
        public const string StateReadOnly = "ReadOnly";
        public const string StateMouseOver = "MouseOver";
        public const string StatePressed = "Pressed";
        public const string StateDisabled = "Disabled";
        public const string GroupFocus = "FocusStates";
        public const string StateUnfocused = "Unfocused";
        public const string StateFocused = "Focused";
        public const string GroupSelection = "SelectionStates";
        public const string StateSelected = "Selected";
        public const string StateUnselected = "Unselected";
        public const string StateSelectedInactive = "SelectedInactive";
        public const string GroupExpansion = "ExpansionStates";
        public const string StateExpanded = "Expanded";
        public const string StateCollapsed = "Collapsed";
        public const string GroupPopup = "PopupStates";
        public const string StatePopupOpened = "PopupOpened";
        public const string StatePopupClosed = "PopupClosed";
        public const string GroupValidation = "ValidationStates";
        public const string StateValid = "Valid";
        public const string StateInvalidFocused = "InvalidFocused";
        public const string StateInvalidUnfocused = "InvalidUnfocused";
        public const string GroupExpandDirection = "ExpandDirectionStates";
        public const string StateExpandDown = "ExpandDown";
        public const string StateExpandUp = "ExpandUp";
        public const string StateExpandLeft = "ExpandLeft";
        public const string StateExpandRight = "ExpandRight";
        public const string GroupHasItems = "HasItemsStates";
        public const string StateHasItems = "HasItems";
        public const string StateNoItems = "NoItems";
        public const string GroupIncrease = "IncreaseStates";
        public const string StateIncreaseEnabled = "IncreaseEnabled";
        public const string StateIncreaseDisabled = "IncreaseDisabled";
        public const string GroupDecrease = "DecreaseStates";
        public const string StateDecreaseEnabled = "DecreaseEnabled";
        public const string StateDecreaseDisabled = "DecreaseDisabled";
        public const string GroupInteractionMode = "InteractionModeStates";
        public const string StateEdit = "Edit";
        public const string StateDisplay = "Display";
        public const string GroupLocked = "LockedStates";
        public const string StateLocked = "Locked";
        public const string StateUnlocked = "Unlocked";
        public const string StateActive = "Active";
        public const string StateInactive = "Inactive";
        public const string GroupActive = "ActiveStates";
        public const string StateUnwatermarked = "Unwatermarked";
        public const string StateWatermarked = "Watermarked";
        public const string GroupWatermark = "WatermarkStates";
        public const string StateCalendarButtonUnfocused = "CalendarButtonUnfocused";
        public const string StateCalendarButtonFocused = "CalendarButtonFocused";
        public const string GroupCalendarButtonFocus = "CalendarButtonFocusStates";
        public const string StateBusy = "Busy";
        public const string StateIdle = "Idle";
        public const string GroupBusyStatus = "BusyStatusStates";
        public const string StateVisible = "Visible";
        public const string StateHidden = "Hidden";
        public const string GroupVisibility = "VisibilityStates";

        public static void GoToState(Control control, bool useTransitions, params string[] stateNames)
        {
            Contract.Requires(control != null, "control should not be null!");
            Contract.Requires(stateNames != null, "stateNames should not be null!");
            Contract.Requires(stateNames.Length > 0, "stateNames should not be empty!");

            foreach (string name in stateNames)
            {
                if (VisualStateManager.GoToState(control, name, useTransitions))
                {
                    break;
                }
            }
        }

        public static FrameworkElement GetImplementationRoot(DependencyObject dependencyObject)
        {
            Contract.Requires(dependencyObject != null, "DependencyObject should not be null.");

            return (1 == VisualTreeHelper.GetChildrenCount(dependencyObject)) ?
                VisualTreeHelper.GetChild(dependencyObject, 0) as FrameworkElement :
                null;
        }

        public static VisualStateGroup TryGetVisualStateGroup(DependencyObject dependencyObject, string groupName)
        {
            FrameworkElement root = GetImplementationRoot(dependencyObject);
            if (root == null)
            {
                return null;
            }

            return VisualStateManager.GetVisualStateGroups(root)
                .OfType<VisualStateGroup>().FirstOrDefault(group => string.CompareOrdinal(groupName, @group.Name) == 0);
        }
    }
}