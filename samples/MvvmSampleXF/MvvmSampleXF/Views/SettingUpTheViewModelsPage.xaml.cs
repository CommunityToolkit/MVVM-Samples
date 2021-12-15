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

            ViewModel = Ioc.Default.GetRequiredService<SettingUpTheViewModelsPageViewModel>();

            BindingContext = ViewModel;
        }

        public SettingUpTheViewModelsPageViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadDocsCommand.Execute("PuttingThingsTogether");
        }
    }
}