using MvvmSample.Core.ViewModels.Widgets;
using CommunityToolkit.Mvvm.DependencyInjection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmSampleXF.Views.Widgets
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostWidget : ContentView
    {
        public PostWidget()
        {
            InitializeComponent();

            BindingContext = Ioc.Default.GetRequiredService<PostWidgetViewModel>();
        }

        public PostWidgetViewModel ViewModel => (PostWidgetViewModel)BindingContext;

        public void OnAppearing()
        {
            ViewModel.IsActive = true;
        }

        public void OnDisappearing()
        {
            ViewModel.IsActive = false;
        }
    }
}