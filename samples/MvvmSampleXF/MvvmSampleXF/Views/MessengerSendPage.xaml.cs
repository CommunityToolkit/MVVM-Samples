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
    public partial class MessengerSendPage : ContentPage
    {
        public MessengerSendPage()
        {
            InitializeComponent();

            ViewModel = Ioc.Default.GetRequiredService<MessengerSendPageViewModel>();

            BindingContext = ViewModel;
        }

        public MessengerSendPageViewModel ViewModel { get; }

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