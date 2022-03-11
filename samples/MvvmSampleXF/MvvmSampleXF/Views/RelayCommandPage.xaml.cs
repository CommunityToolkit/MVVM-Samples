using MvvmSample.Core.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmSampleXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RelayCommandPage : ContentPage
    {
        public RelayCommandPage()
        {
            InitializeComponent();

            BindingContext = Ioc.Default.GetRequiredService<RelayCommandPageViewModel>();
        }

        public RelayCommandPageViewModel ViewModel => (RelayCommandPageViewModel)BindingContext;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadDocsCommand.Execute("RelayCommand");
        }
    }
}