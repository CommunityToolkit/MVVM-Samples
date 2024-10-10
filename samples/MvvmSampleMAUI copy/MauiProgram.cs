using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using MvvmSample.Core.Services;
using MvvmSample.Core.ViewModels;
using MvvmSample.Core.ViewModels.Widgets;
using MvvmSampleMAUI.Services;
using MvvmSampleMAUI.Views;
using Polly;
using Refit;

namespace MvvmSampleMAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<IFilesService, FileService>()
			.AddSingleton<ISettingsService, SettingsService>()
			.AddSingleton(RestService.For<IRedditService>("https://www.reddit.com/"))
			.AddTransientWithShellRoute<AsyncRelayCommandPage, AsyncRelayCommandPageViewModel>()
			.AddTransientWithShellRoute<BuildingTheUIPage, SamplePageViewModel>()
			.AddTransientWithShellRoute<IntroductionPage, ObservableObjectPageViewModel>()
			.AddTransientWithShellRoute<IoCPage, IocPageViewModel>()
			.AddTransientWithShellRoute<MessengerPage, MessengerPageViewModel>()
			.AddTransientWithShellRoute<MessengerRequestPage, MessengerPageViewModel>()
			.AddTransientWithShellRoute<MessengerSendPage, MessengerPageViewModel>()
			.AddTransientWithShellRoute<ObservableObjectPage, ObservableObjectPageViewModel>()
			.AddTransientWithShellRoute<PuttingThingsTogetherPage, SamplePageViewModel>()
			.AddTransientWithShellRoute<RedditBrowserPage>()
			.AddTransientWithShellRoute<RedditServicePage, SamplePageViewModel>()
			.AddTransientWithShellRoute<RelayCommandPage, RelayCommandPageViewModel>()
			.AddTransientWithShellRoute<SettingsServicePage, SamplePageViewModel>()
			.AddTransientWithShellRoute<MessengerSendPage, MessengerPageViewModel>()
			.AddTransientWithShellRoute<MessengerSendPage, MessengerPageViewModel>()
			.AddTransientWithShellRoute<MessengerSendPage, MessengerPageViewModel>()
			.AddTransientWithShellRoute<MessengerSendPage, MessengerPageViewModel>()
			.AddTransientWithShellRoute<MessengerSendPage, MessengerPageViewModel>();

		return builder.Build();
	}

	static IServiceCollection AddTransientWithShellRoute<TPage>(this IServiceCollection services) where TPage : ContentPage
	{
		Routing.RegisterRoute(AppShell.GetPageRoute<TPage>(), typeof(TPage));
		return services.AddTransient<TPage>();
	}

	static IServiceCollection AddTransientWithShellRoute<TPage, TViewModel>(this IServiceCollection services) where TPage : BaseContentPage<TViewModel>
		where TViewModel : ObservableObject
	{
		return services.AddTransientWithShellRoute<TPage, TViewModel>(AppShell.GetPageRoute<TPage>());
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