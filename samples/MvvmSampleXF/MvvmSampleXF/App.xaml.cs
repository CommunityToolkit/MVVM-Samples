using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MvvmSample.Core.Services;
using MvvmSampleXF.Services;
using RestEase;
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
                    .AddSingleton<IFilesService, FileService>()
                    .AddSingleton<ISettingsService, SettingsService>()
                    .AddSingleton(RestClient.For<IRedditService>("https://www.reddit.com/"))
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
