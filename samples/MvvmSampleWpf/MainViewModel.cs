using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using MvvmSample.Core.ViewModels;
using System.Windows.Input;

namespace MvvmSampleWpf
{
    public class MainViewModel : ObservableObject
    {
        private ObservableObject? _selectedViewModel;

        public MainViewModel()
        {
            ShowIntroductionViewModel = new RelayCommand(() => SelectedViewModel = Ioc.Default.GetRequiredService<IntroductionPageViewModel>());
            ShowObservableObjectViewModel = new RelayCommand(() => SelectedViewModel = Ioc.Default.GetRequiredService<ObservableObjectPageViewModel>());            
            ShowCommandsViewModel = new RelayCommand(() => SelectedViewModel = Ioc.Default.GetRequiredService<RelayCommandPageViewModel>());
            ShowAsyncCommandsViewModel = new RelayCommand(() => SelectedViewModel = Ioc.Default.GetRequiredService<AsyncRelayCommandPageViewModel>());
            ShowMessengerViewModel = new RelayCommand(() => SelectedViewModel = Ioc.Default.GetRequiredService<MessengerPageViewModel>());
            ShowSendMessageViewModel = new RelayCommand(() => SelectedViewModel = Ioc.Default.GetRequiredService<MessengerSendPageViewModel>());            
            ShowRequestMessageViewModel = new RelayCommand(() => SelectedViewModel = Ioc.Default.GetRequiredService<MessengerRequestPageViewModel>());
            ShowIocViewModel = new RelayCommand(() => SelectedViewModel = Ioc.Default.GetRequiredService<IocPageViewModel>());
            ShowPutTogetherViewModel = new RelayCommand(() => SelectedViewModel = Ioc.Default.GetRequiredService<PuttingThingsTogetherPageViewModel>());
            ShowSetupViewModel = new RelayCommand(() => SelectedViewModel = Ioc.Default.GetRequiredService<SettingUpTheViewModelsPageViewModel>());
            ShowSettingsServiceViewModel = new RelayCommand(() => SelectedViewModel = Ioc.Default.GetRequiredService<SettingsServicePageViewModel>());
            ShowRedditServiceViewModel = new RelayCommand(() => SelectedViewModel = Ioc.Default.GetRequiredService<RedditServicePageViewModel>());            
            ShowUIViewModel = new RelayCommand(() => SelectedViewModel = Ioc.Default.GetRequiredService<BuildingTheUIPageViewModel>());
            ShowReddibBrowserMessageViewModel = new RelayCommand(() => SelectedViewModel = Ioc.Default.GetRequiredService<RedditBrowserMessagePageViewModel>());
            ShowReddibBrowserViewModel = new RelayCommand(() => SelectedViewModel = Ioc.Default.GetRequiredService<RedditBrowserPageViewModel>());

            SelectedViewModel = Ioc.Default.GetRequiredService<IntroductionPageViewModel>();
        }

        public ICommand ShowIntroductionViewModel { get; }
        public ICommand ShowObservableObjectViewModel { get; }
        public ICommand ShowCommandsViewModel { get; }
        public ICommand ShowAsyncCommandsViewModel { get; }
        public ICommand ShowMessengerViewModel { get; }
        public ICommand ShowSendMessageViewModel { get; }
        public ICommand ShowRequestMessageViewModel { get; }
        public ICommand ShowIocViewModel { get; }
        public ICommand ShowPutTogetherViewModel { get; }
        public ICommand ShowSetupViewModel { get; }
        public ICommand ShowSettingsServiceViewModel { get; }
        public ICommand ShowRedditServiceViewModel { get; }
        public ICommand ShowUIViewModel { get; }
        public ICommand ShowReddibBrowserMessageViewModel { get; }
        public ICommand ShowReddibBrowserViewModel { get; }

        public ObservableObject? SelectedViewModel 
        { 
            get => _selectedViewModel; 
            set => SetProperty(ref _selectedViewModel, value);
        }
    }
}
