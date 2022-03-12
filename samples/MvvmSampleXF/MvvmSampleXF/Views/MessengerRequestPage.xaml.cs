using MvvmSample.Core.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmSampleXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessengerRequestPage : ContentPage
    {
        public MessengerRequestPage()
        {
            InitializeComponent();
        }

        public MessengerPageViewModel ViewModel => (MessengerPageViewModel)BindingContext;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadDocsCommand.Execute("Messenger");
            ViewModel.SenderViewModel.IsActive = true;
            ViewModel.ReceiverViewModel.IsActive = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ViewModel.SenderViewModel.IsActive = false;
            ViewModel.ReceiverViewModel.IsActive = false;
        }
    }
}