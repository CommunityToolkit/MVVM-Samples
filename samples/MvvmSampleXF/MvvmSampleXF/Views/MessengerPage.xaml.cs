using MvvmSample.Core.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmSampleXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessengerPage : ContentPage
    {
        public MessengerPage()
        {
            InitializeComponent();

            BindingContext = Ioc.Default.GetRequiredService<MessengerPageViewModel>();
        }

        public MessengerPageViewModel ViewModel => (MessengerPageViewModel)BindingContext;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadDocsCommand.Execute("Messenger");
        }
    }
}