using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using MvvmSample.Core.Services;
using MvvmSample.Core.ViewModels;
using MvvmSample.Core.ViewModels.Widgets;
using MvvmSampleMAUI.Controls;
using MvvmSampleMAUI.Services;
using MvvmSampleMAUI.Views;
using Polly;
using Refit;

namespace MvvmSampleMAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMarkup()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("FontAwesomeRegular.otf", "FontAwesomeRegular");
                fonts.AddFont("FontAwesomeSolid.otf", "FontAwesomeSolid");
                fonts.AddFont("FontAwesomeBrands.otf", "FontAwesomeBrands");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        RegisterServices(builder.Services);
        RegisterViews(builder.Services);
        RegisterViewModels(builder.Services);

        return builder.Build();
    }

    static void RegisterServices(in IServiceCollection services)
    {
        services.AddSingleton<IFilesService, FileService>();
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddRefitClient<IRedditService>()
            .ConfigureHttpClient(static client => client.BaseAddress = new Uri("https://www.reddit.com"))
            .AddStandardResilienceHandler(static options => options.Retry = new MobileHttpRetryStrategyOptions());
    }

    static void RegisterViews(in IServiceCollection services)
    {
        services.AddTransient<AsyncRelayCommandPage>();
        services.AddTransient<BuildingTheUIPage>();
        services.AddTransient<FlyoutHeader>();
        services.AddTransient<InteractiveSample>();
        services.AddTransient<IntroductionPage>();
        services.AddTransient<IoCPage>();
        services.AddTransient<MessengerPage>();
        services.AddTransient<MessengerRequestPage>();
        services.AddTransient<MessengerSendPage>();
        services.AddTransient<ObservableObjectPage>();
        services.AddTransient<PuttingThingsTogetherPage>();
        services.AddTransient<RedditBrowserPage>();
        services.AddTransient<RedditServicePage>();
        services.AddTransient<RelayCommandPage>();
        services.AddTransient<SettingsServicePage>();
        services.AddSingleton<AppShell>();
    }

    static void RegisterViewModels(in IServiceCollection services)
    {
        services.AddTransient<AsyncRelayCommandPageViewModel>();
        services.AddTransient<ContactsListWidgetViewModel>();
        services.AddTransient<IocPageViewModel>();
        services.AddTransient<MessengerPageViewModel>();
        services.AddTransient<ObservableObjectPageViewModel>();
        services.AddTransient<PostWidgetViewModel>();
        services.AddTransient<RelayCommandPageViewModel>();
        services.AddTransient<SamplePageViewModel>();
        services.AddTransient<SubredditWidgetViewModel>();
        services.AddTransient<ValidationFormWidgetViewModel>();
    }

    sealed class MobileHttpRetryStrategyOptions : HttpRetryStrategyOptions
    {
        public MobileHttpRetryStrategyOptions()
        {
            BackoffType = DelayBackoffType.Exponential;
            MaxRetryAttempts = 3;
            UseJitter = true;
            Delay = TimeSpan.FromSeconds(2);
        }
    }
}