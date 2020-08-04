using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MvvmSampleUwp.Views;
using NavigationView = Microsoft.UI.Xaml.Controls.NavigationView;
using NavigationViewItem = Microsoft.UI.Xaml.Controls.NavigationViewItem;
using NavigationViewBackRequestedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewBackRequestedEventArgs;
using NavigationViewItemInvokedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs;

namespace MvvmSampleUwp
{
    public sealed partial class Shell : UserControl
    {
        private readonly IReadOnlyCollection<(NavigationViewItem Item, Type PageType)> NavigationItems;

        public Shell()
        {
            this.InitializeComponent();

            NavigationItems = new[]
            {
                (IntroductionItem, typeof(IntroductionPage)),
                (ObservableObjectItem, typeof(ObservableObjectPage)),
                (CommandsItem, typeof(RelayCommandPage)),
                (AsyncCommandsItem, typeof(AsyncRelayCommandPage)),
                (MessengerItem, typeof(MessengerPage)),
                (SendMessagesItem, typeof(MessengerSendPage)),
                (RequestMessagesItem, typeof(MessengerRequestPage)),
                (InversionOfControlItem, typeof(IocPage))
            };

            // Set the custom title bar to act as a draggable region
            Window.Current.SetTitleBar(TitleBarBorder);
        }

        // Navigates to a sample page when a button is clicked
        private void NavigationView_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (NavigationItems.FirstOrDefault(item => item.Item == args.InvokedItemContainer).PageType is Type pageType)
            {
                NavigationFrame.Navigate(pageType);
            }
        }

        // Sets whether or not the back button is enabled
        private void NavigationFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            NavigationView.IsBackEnabled = ((Frame)sender).BackStackDepth > 0;
        }

        // Navigates back
        private void NavigationView_OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (NavigationFrame.BackStack.LastOrDefault() is PageStackEntry entry)
            {
                NavigationView.SelectedItem = NavigationItems.First(item => item.PageType == entry.SourcePageType).Item;

                NavigationFrame.GoBack();
            }
        }

        // Select the introduction item when the shell is loaded
        private void Shell_OnLoaded(object sender, RoutedEventArgs e)
        {
            NavigationView.SelectedItem = IntroductionItem;

            NavigationFrame.Navigate(typeof(IntroductionPage));
        }
    }
}
