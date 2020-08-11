// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MvvmSampleUwp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MessengerRequestPage : Page
    {
        public MessengerRequestPage()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.SenderViewModel.IsActive = true;
            ViewModel.ReceiverViewModel.IsActive = true;
        }

        /// <inheritdoc/>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.SenderViewModel.IsActive = false;
            ViewModel.ReceiverViewModel.IsActive = false;
        }

        private void RequestCurrentUsername_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.RequestCurrentUsername();
        }
private void ResetCurrentUsername_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.ResetCurrentUsername();
        }
        
    }
}
