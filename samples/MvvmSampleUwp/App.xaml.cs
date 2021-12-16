﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MvvmSampleUwp.Services;
using Refit;
using MvvmSampleUwp.Helpers;
using MvvmSample.Core.Services;
using MvvmSample.Core.ViewModels.Widgets;
using MvvmSample.Core.ViewModels;
using System.Linq;

namespace MvvmSampleUwp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default <see cref="Application"/> class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // Ensure the UI is initialized
            if (Window.Current.Content is null)
            {
                Window.Current.Content = new Shell();

                TitleBarHelper.StyleTitleBar();
                TitleBarHelper.ExpandViewIntoTitleBar();

                var serviceProvider = new ServiceCollection()
                    //Services
                    .AddSingleton<IFilesService, FilesService>()
                    .AddSingleton<ISettingsService, SettingsService>()
                    .AddSingleton(RestService.For<IRedditService>("https://www.reddit.com/"))
                    //ViewModels
                    .AddTransient<PostWidgetViewModel>()
                    .AddTransient<SubredditWidgetViewModel>()
                    .AddTransient<AsyncRelayCommandPageViewModel>()
                    .AddTransient<IocPageViewModel>()
                    .AddTransient<MessengerPageViewModel>()
                    .AddTransient<ObservableObjectPageViewModel>()
                    .AddTransient<RelayCommandPageViewModel>()
                    .AddTransient<SamplePageViewModel>()
                    .BuildServiceProvider();

                // Register services
                Ioc.Default.ConfigureServices(serviceProvider);

                ViewModelLocator.SetViewModelFactory(view =>
                {
                    var viewName = view.GetType().Name;
                    var viewModelName = $"{viewName}ViewModel";
                    var viewModelType = typeof(SamplePageViewModel).Assembly.GetTypes().Where(x => x.Name == viewModelName).FirstOrDefault();

                    if (viewModelType == null)
                    {
                        if (viewModelName.Contains("Messenger"))
                            viewModelType = typeof(MessengerPageViewModel);
                        else
                            viewModelType = typeof(SamplePageViewModel);
                    }

                    return serviceProvider.GetService(viewModelType);
                });
            }

            // Enable the prelaunch if needed, and activate the window
            if (e.PrelaunchActivated == false)
            {
                CoreApplication.EnablePrelaunch(true);

                Window.Current.Activate();
            }
        }
    }
}
