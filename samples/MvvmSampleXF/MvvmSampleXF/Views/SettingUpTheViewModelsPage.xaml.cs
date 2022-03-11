using MvvmSample.Core.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmSampleXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingUpTheViewModelsPage : ContentPage
    {
        public SettingUpTheViewModelsPage()
        {
            InitializeComponent();

            BindingContext = Ioc.Default.GetRequiredService<SamplePageViewModel>();
        }

        public SamplePageViewModel ViewModel => (SamplePageViewModel)BindingContext;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadDocsCommand.Execute("PuttingThingsTogether");
        }
    }
}