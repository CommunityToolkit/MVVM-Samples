// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Mvvm.DependencyInjection;
using MvvmSample.Core.ViewModels;
using Windows.UI.Xaml.Controls;

namespace MvvmSampleUwp.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ObservablePropertyAttributePage : Page
{
    public ObservablePropertyAttributePage()
    {
        this.InitializeComponent();

        DataContext = Ioc.Default.GetRequiredService<SamplePageViewModel>();
    }

    public SamplePageViewModel ViewModel => (SamplePageViewModel)DataContext;
}
