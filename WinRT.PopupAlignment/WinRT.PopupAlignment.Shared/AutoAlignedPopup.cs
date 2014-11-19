using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace WinRT.PopupAlignment
{
    public class AutoAlignedPopup : FrameworkElement
    {
        public enum RelativeDirection { Top, Bottom, Left, Right }

        public static readonly DependencyProperty PopupProperty =
            DependencyProperty.Register("PopupProperty", typeof (Popup), typeof (AutoAlignedPopup), new PropertyMetadata(null));

        public Popup Popup
        {
            get { return (Popup) GetValue(PopupProperty); }
            private set { SetValue(PopupProperty, value); }
        } 

        public AutoAlignedPopup(Popup popup)
        {
            if (popup == null) throw new ArgumentNullException("popup");
            if (popup.Child  == null) throw new ArgumentException("popup.Child");

            Popup = popup;
        }

        public void Close()
        {
            if (Popup == null) return;

            Popup.IsOpen = false;
        }

        /// <summary>
        /// Opens the Flyout relative to another element. If the flyout is opened off screen, it will automatically be re-aligned to be fully visible.
        /// </summary>
        /// <param name="relativeElement">The element where the popup should be opened relative to.</param>
        /// <param name="relativeDirection">The direction to open the flyout relative to the element.</param>
        /// <param name="relativeContainer">Optional: The relative element's container. If not set, the Page will be used.</param>
        public void Open(FrameworkElement relativeElement, RelativeDirection relativeDirection, FrameworkElement relativeContainer = null)
        {
            // Make sure an elemenet was passed in
            if (relativeElement == null) throw new ArgumentNullException("relativeElement");

            // If a container was not passed in, locate the current page in the window
            if (relativeContainer == null)
            {
                var frame = Window.Current.Content as Frame;
                if (frame != null)
                    relativeContainer = frame.Content as Page;
            }

            // Ensure the Popup's child is a Framework element, so we can locate bounds.
            var popupContent = Popup.Child as FrameworkElement;
            if (popupContent == null) throw new NullReferenceException("Popup.Child must be a FrameworkElement");

            // Measure & Arrange the border to get exact dimensions of its content size. 
            // This step is necessary because the pop up is not visible, the content has not been measured, meaning it's height/width are both 0.
            popupContent.Measure(new Size(Window.Current.Bounds.Width, Window.Current.Bounds.Height));
            popupContent.Arrange(new Rect(0, 0, popupContent.DesiredSize.Width, popupContent.DesiredSize.Height));

            // Get the button bounds to set our starting point for the pop up
            var relativeBounds = relativeElement.GetElementBounds(relativeContainer);

            // Based on the relative direction, set the offsets
            switch (relativeDirection)
            {
                case RelativeDirection.Top:
                    // Open the popup above, so the offset is the top of the relative element minus the height of the popup content
                    Popup.VerticalOffset = relativeBounds.Top - popupContent.DesiredSize.Height;
                    Popup.HorizontalOffset = relativeBounds.Left;
                    break;
                case RelativeDirection.Bottom:
                    // Open the popup below, so the offsets are just the bottom/left sides of the relative element
                    Popup.VerticalOffset = relativeBounds.Bottom;
                    Popup.HorizontalOffset = relativeBounds.Left;
                    break;
                case RelativeDirection.Left:
                    // Open the popup to the left, so the offset is the left of the relative element minus the width of the popup content
                    Popup.HorizontalOffset = relativeBounds.Left - popupContent.DesiredSize.Width;
                    Popup.VerticalOffset = relativeBounds.Top;
                    break;
                case RelativeDirection.Right:
                    // Open the popup to the right, so the offsets are just the right/top sides of the relative element
                    Popup.HorizontalOffset = relativeBounds.Right;
                    Popup.VerticalOffset = relativeBounds.Top;
                    break;
            }

            // Open the popup
            Popup.IsOpen = true;

            // Update the pages layout so the popup content is measured
            UpdateLayout();

            // Get the borders bounds so we can determine which offset needs fixing
            var popupContentBounds = popupContent.GetElementBounds(relativeContainer);
            var containerBounds = new Rect(0, 0, relativeContainer.ActualWidth, relativeContainer.ActualHeight);

            // Using the container & the popup content dimensions, determine if this element is off screen in any direction.
            // If so, determine by how far, and adjust the offset to accomodate.
            if (popupContentBounds.Top <= containerBounds.Top)
            {
                var offset = containerBounds.Top - popupContentBounds.Top;

                Popup.VerticalOffset = Popup.VerticalOffset - offset;
            }

            if (popupContentBounds.Bottom >= containerBounds.Bottom)
            {
                var offset = popupContentBounds.Bottom - containerBounds.Bottom;

                Popup.VerticalOffset = Popup.VerticalOffset - offset;
            }

            if (popupContentBounds.Left <= containerBounds.Left)
            {
                var offset = containerBounds.Left - popupContentBounds.Left;

                Popup.HorizontalOffset = Popup.HorizontalOffset - offset;
            }

            if (popupContentBounds.Right >= containerBounds.Right)
            {
                var offset = popupContentBounds.Right - containerBounds.Right;

                Popup.HorizontalOffset = Popup.HorizontalOffset - offset;
            }
        }
    }
}
