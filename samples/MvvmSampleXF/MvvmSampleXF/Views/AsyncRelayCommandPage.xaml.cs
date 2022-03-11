using MvvmSample.Core.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;

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

            BindingContext = Ioc.Default.GetRequiredService<AsyncRelayCommandPageViewModel>();
        }

        public AsyncRelayCommandPageViewModel ViewModel => (AsyncRelayCommandPageViewModel)BindingContext;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadDocsCommand.Execute("AsyncRelayCommand");
        }
    }
}