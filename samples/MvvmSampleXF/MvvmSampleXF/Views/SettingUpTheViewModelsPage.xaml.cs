using MvvmSample.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xam.Forms.Markdown;
using CommunityToolkit.Mvvm.DependencyInjection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmSampleXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingUpTheViewModelsPage : ContentPage
    {
        public SettingUpTheViewModelsPage()
        {
            InitializeComponent();
        }

        public SamplePageViewModel ViewModel => BindingContext as SamplePageViewModel;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadDocsCommand.Execute("PuttingThingsTogether");
        }
    }
}