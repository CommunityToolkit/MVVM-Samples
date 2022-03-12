using MvvmSample.Core.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmSampleXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuildingTheUIPage : ContentPage
    {
        public BuildingTheUIPage()
        {
            InitializeComponent();
        }

        public SamplePageViewModel ViewModel => (SamplePageViewModel)BindingContext;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadDocsCommand.Execute("PuttingThingsTogether");
        }
    }
}