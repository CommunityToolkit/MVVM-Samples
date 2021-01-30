// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml.Controls;

namespace MvvmSample.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ObservableObjectPage : Page
    {
        public ObservableObjectPage()
        {
            this.InitializeComponent();
        }

        private void ReloadTask_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.ReloadTask();
        }
    }
}
