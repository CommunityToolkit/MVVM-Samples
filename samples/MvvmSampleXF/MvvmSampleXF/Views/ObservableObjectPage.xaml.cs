using MvvmSample.Core.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmSampleXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ObservableObjectPage : ContentPage
    {
        public ObservableObjectPage()
        {
            InitializeComponent();
        }

        public ObservableObjectPageViewModel ViewModel => (ObservableObjectPageViewModel)BindingContext;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadDocsCommand.Execute("ObservableObject");
        }
    }
}