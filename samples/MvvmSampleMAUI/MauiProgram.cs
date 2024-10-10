using CommunityToolkit.Maui;
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
			.AddRefitClient<IRedditService>()
			.ConfigureHttpClient(static client => client.BaseAddress = new Uri("https://www.reddit.com"))
			.AddStandardResilienceHandler(static options => options.Retry = new MobileHttpRetryStrategyOptions());

		RegisterViews(builder.Services);
		RegisterViewModels(builder.Services);

		return builder.Build();
	}

	static void RegisterViews(in IServiceCollection services)
	{
		services.AddTransient<AsyncRelayCommandPage>()
			.AddTransient<BuildingTheUIPage>()
			.AddTransient<FlyoutHeader>()
			.AddTransient<InteractiveSample>()
			.AddTransient<IntroductionPage>()
			.AddTransient<IoCPage>()
			.AddTransient<MessengerPage>()
			.AddTransient<MessengerRequestPage>()
			.AddTransient<MessengerSendPage>()
			.AddTransient<ObservableObjectPage>()
			.AddTransient<PuttingThingsTogetherPage>()
			.AddTransient<RedditBrowserPage>()
			.AddTransient<RedditServicePage>()
			.AddTransient<RelayCommandPage>()
			.AddTransient<SettingsServicePage>();
	}

	static void RegisterViewModels(in IServiceCollection services)
	{
		services.AddTransient<AsyncRelayCommandPageViewModel>()
			.AddTransient<ContactsListWidgetViewModel>()
			.AddTransient<IocPageViewModel>()
			.AddTransient<MessengerPageViewModel>()
			.AddTransient<ObservableObjectPageViewModel>()
			.AddTransient<PostWidgetViewModel>()
			.AddTransient<RelayCommandPageViewModel>()
			.AddTransient<SamplePageViewModel>()
			.AddTransient<SubredditWidgetViewModel>()
			.AddTransient<ValidationFormWidgetViewModel>();
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