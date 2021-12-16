using MvvmSample.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;

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

        public ObservableObjectPageViewModel ViewModel => BindingContext as ObservableObjectPageViewModel;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadDocsCommand.Execute("ObservableObject");
        }
    }
}