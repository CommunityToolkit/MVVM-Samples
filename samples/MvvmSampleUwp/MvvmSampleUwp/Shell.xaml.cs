using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NavigationView = Microsoft.UI.Xaml.Controls.NavigationView;
using NavigationViewItemInvokedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs;

namespace MvvmSampleUwp
{
    public sealed partial class Shell : UserControl
    {
        public Shell()
        {
            this.InitializeComponent();

            // Set the custom title bar to act as a draggable region
            Window.Current.SetTitleBar(TitleBarBorder);
        }

        // Navigates to a sample page when a button is clicked
        private void NavigationView_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer.Tag is string pageTypeName)
            {
                Type pageType = Type.GetType($"MvvmSampleUwp.Views.{pageTypeName}");

                NavigationFrame.Navigate(pageType);
            }
        }
    }
}
