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
    public partial class MessengerRequestPage : ContentPage
    {
        public MessengerRequestPage()
        {
            InitializeComponent();

            ViewModel = Ioc.Default.GetRequiredService<MessengerRequestPageViewModel>();

            BindingContext = ViewModel;
        }

        public MessengerRequestPageViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadDocsCommand.Execute("Messenger");
            ViewModel.Loaded();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ViewModel.Unloaded();            
        }
    }
}