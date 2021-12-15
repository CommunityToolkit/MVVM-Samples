// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Mvvm.DependencyInjection;
using MvvmSample.Core.ViewModels;
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

            ViewModel = Ioc.Default.GetRequiredService<MessengerRequestPageViewModel>();

            DataContext = ViewModel;
        }

        public MessengerRequestPageViewModel ViewModel { get; }

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Loaded();
        }

        /// <inheritdoc/>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Unloaded();
        }
    }
}
