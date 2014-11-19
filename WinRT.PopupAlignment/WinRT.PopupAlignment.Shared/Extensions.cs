using Windows.Foundation;
using Windows.UI.Xaml;

namespace WinRT.PopupAlignment
{
    public static class Extensions
    {
        /// <summary>
        /// Returns a Rectangle instance which includes the bounds for an element within its outer container.
        /// </summary>
        /// <param name="element">The element whose bounds you wish to retieve</param>
        /// <param name="container">The elements container. Use page if there is no direct container</param>
        /// <returns></returns>
        public static Rect GetElementBounds(this FrameworkElement element, FrameworkElement container)
        {
            if (element == null || container == null)
                return Rect.Empty;

            if (element.Visibility != Visibility.Visible)
                return Rect.Empty;

            return element.TransformToVisual(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
        }
    }
}
