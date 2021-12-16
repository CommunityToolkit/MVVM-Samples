using MvvmSample.Core.ViewModels;

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
        }

        public RelayCommandPageViewModel ViewModel => BindingContext as RelayCommandPageViewModel;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadDocsCommand.Execute("RelayCommand");
        }
    }
}