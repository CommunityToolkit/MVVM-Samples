using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MvvmSample.Core.Services;
using MvvmSample.Core.ViewModels;
using MvvmSample.Core.ViewModels.Widgets;
using MvvmSample.Core.ViewModels.Widgets.Messages;
using MvvmSample.Core.ViewModels.Widgets.NoMessages;
using MvvmSampleXF.Services;
using Refit;
using Xamarin.Forms;

namespace MvvmSampleXF
{
    public partial class App : Application
    {
        private bool _initialized;

        public App()
        {
            InitializeComponent();

            // Register services
            if (!_initialized)
            {
                _initialized = true;
                Ioc.Default.ConfigureServices(
                    new ServiceCollection()
                    //Services
                    .AddSingleton<IFilesService, FileService>()
                    .AddSingleton<ISettingsService, SettingsService>()
                    .AddSingleton(RestService.For<IRedditService>("https://www.reddit.com/"))
                    //ViewModels
                    .AddTransient<PostWidgetMessageViewModel>()
                    .AddTransient<SubredditWidgetMessageViewModel>()
                    .AddTransient<PostWidgetViewModel>()
                    .AddTransient<SubredditWidgetViewModel>()
                    .AddTransient<AsyncRelayCommandPageViewModel>()
                    .AddTransient<IocPageViewModel>()
                    .AddTransient<MessengerPageViewModel>()
                    .AddTransient<ObservableObjectPageViewModel>()
                    .AddTransient<RelayCommandPageViewModel>()
                    .AddTransient<SamplePageViewModel>()
                    .AddTransient<RedditBrowserPageViewModel>()
                    .BuildServiceProvider());
            }

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
