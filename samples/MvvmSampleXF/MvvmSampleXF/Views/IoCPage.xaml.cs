using MvvmSample.Core.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmSampleXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IoCPage : ContentPage
    {
        public IoCPage()
        {
            InitializeComponent();

            BindingContext = Ioc.Default.GetRequiredService<IocPageViewModel>();
        }

        public IocPageViewModel ViewModel => (IocPageViewModel)BindingContext;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadDocsCommand.Execute("IoC");
        }
    }
}