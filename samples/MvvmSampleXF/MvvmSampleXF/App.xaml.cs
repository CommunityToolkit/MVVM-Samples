using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MvvmSample.Core.Services;
using MvvmSample.Core.ViewModels;
using MvvmSample.Core.ViewModels.Widgets;
using MvvmSampleXF.Services;
using Refit;
using System.Linq;
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

                var serviceProvider = new ServiceCollection()
                    //Services
                    .AddSingleton<IFilesService, FileService>()
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
