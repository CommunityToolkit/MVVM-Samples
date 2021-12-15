using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MvvmSample.Core.Services;
using MvvmSample.Core.ViewModels;
using MvvmSample.Core.ViewModels.Widgets.Messages;
using MvvmSample.Core.ViewModels.Widgets.NoMessages;
using MvvmSampleWpf.Services;
using MvvmSampleWpf.Views;
using MvvmSampleWpf.Views.Widgets.NoMessages;
using Refit;
using System.Windows;

namespace MvvmSampleWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //var mappingViewFactory = new MappingViewFactory()
            //    //Pages
            //    .Register<ObservableObjectView, ObservableObjectPageViewModel>()
            //    .Register<IntroductionView, IntroductionPageViewModel>()
            //    .Register<RelayCommandView, RelayCommandPageViewModel>()
            //    .Register<AsyncRelayCommandView, AsyncRelayCommandPageViewModel>()
            //    .Register<MessengerView, MessengerPageViewModel>()
            //    .Register<MessengerSendView, MessengerSendPageViewModel>()
            //    .Register<MessengerRequestView, MessengerRequestPageViewModel>()
            //    .Register<IocView, IocPageViewModel>()
            //    .Register<PuttingThingsTogetherView, PuttingThingsTogetherPageViewModel>()
            //    .Register<SettingUpTheViewModelsView, SettingUpTheViewModelsPageViewModel>()
            //    .Register<SettingsServiceView, SettingsServicePageViewModel>()
            //    .Register<RedditServiceView, RedditServicePageViewModel>()
            //    .Register<BuildingTheUIView, BuildingTheUIPageViewModel>()
            //    .Register<RedditBrowserMessageView, RedditBrowserMessagePageViewModel>()
            //    .Register<RedditBrowserView, RedditBrowserPageViewModel>()
            //    //Widgets
            //    .Register<SubredditWidget, SubredditWidgetViewModel>()
            //    .Register<PostWidget, PostWidgetViewModel>()
            //    ;

            var namingConvenionViewFactory = new NamingConvenionViewFactory();

            Ioc.Default.ConfigureServices(
                    new ServiceCollection()
                    //Services
                    .AddSingleton<IFilesService, FilesService>()
                    .AddSingleton<ISettingsService, SettingsService>()
                    .AddSingleton(RestService.For<IRedditService>("https://www.reddit.com/"))
                    //Widget ViewModels
                    .AddTransient<PostWidgetViewModel>()
                    .AddTransient<SubredditWidgetViewModel>()
                    .AddTransient<PostWidgetMessageViewModel>()
                    .AddTransient<SubredditWidgetMessageViewModel>()
                    //Page ViewModels
                    .AddTransient<IntroductionPageViewModel>()
                    .AddTransient<AsyncRelayCommandPageViewModel>()
                    .AddTransient<IocPageViewModel>()
                    .AddTransient<MessengerPageViewModel>()
                    .AddTransient<ObservableObjectPageViewModel>()
                    .AddTransient<RelayCommandPageViewModel>()
                    .AddTransient<RedditBrowserPageViewModel>()
                    .AddTransient<MessengerSendPageViewModel>()
                    .AddTransient<MessengerRequestPageViewModel>()
                    .AddTransient<PuttingThingsTogetherPageViewModel>()
                    .AddTransient<SettingUpTheViewModelsPageViewModel>()
                    .AddTransient<SettingsServicePageViewModel>()
                    .AddTransient<RedditServicePageViewModel>()
                    .AddTransient<BuildingTheUIPageViewModel>()
                    .AddTransient<RedditBrowserMessagePageViewModel>()
                    //WPF
                    .AddSingleton<MainViewModel>()
                    //.AddSingleton<IViewFactory>(mappingViewFactory)
                    .AddSingleton<IViewFactory>(namingConvenionViewFactory)
                    .BuildServiceProvider());
        }
    }
}
