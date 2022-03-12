using MvvmSample.Core.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmSampleXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AsyncRelayCommandPage : ContentPage
    {
        public AsyncRelayCommandPage()
        {
            InitializeComponent();
        }

        public AsyncRelayCommandPageViewModel ViewModel => (AsyncRelayCommandPageViewModel)BindingContext;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadDocsCommand.Execute("AsyncRelayCommand");
        }
    }
}