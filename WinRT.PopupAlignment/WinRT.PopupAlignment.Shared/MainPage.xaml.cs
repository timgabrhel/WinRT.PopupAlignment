using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WinRT.PopupAlignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Popup popup;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Default_Click(object sender, RoutedEventArgs e)
        {
            // Show the popup with it's defaults (no offset)
            Popup1.HorizontalOffset = 0;
            Popup1.VerticalOffset = 0;
            Popup1.IsOpen = true;
        }

        private void Offset_Click(object sender, RoutedEventArgs e)
        {
            // Hardcode the offsets to position the popup
            Popup1.HorizontalOffset = 250;
            Popup1.VerticalOffset = 250;
            Popup1.IsOpen = true;
        }

        private void Relative_Click(object sender, RoutedEventArgs e)
        {
            // Show the popup relative (bottom) to this button
            var button = sender as Button;
            if (button == null) return;

            var buttonBounds = button.GetElementBounds(this);

            // Set the VerticalOffset (distance from top) equal to the bottom of the button
            Popup1.VerticalOffset = buttonBounds.Bottom;

            // Set the HorizontalOffset (distance from left) equal to the left of the button
            Popup1.HorizontalOffset = buttonBounds.Left;

            Popup1.IsOpen = true;
        }

        private void RelativeOffScreen_Click(object sender, RoutedEventArgs e)
        {
            // Show the popup relative (right) to this button
            var button = sender as Button;
            if (button == null) return;

            var buttonBounds = button.GetElementBounds(this);

            // Set the VerticalOffset (distance from top) equal to the bottom of the button
            Popup1.VerticalOffset = buttonBounds.Top;

            // Set the HorizontalOffset (distance from left) equal to the left of the button
            Popup1.HorizontalOffset = buttonBounds.Right;

            Popup1.IsOpen = true;
        }

        private void RelativeAutoAlign_Click(object sender, RoutedEventArgs e)
        {
            var aap = new AutoAlignedPopup(Popup1);
            aap.Open(sender as FrameworkElement, AutoAlignedPopup.RelativeDirection.Right);
        }
    }
}
