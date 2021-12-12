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
    public partial class MessengerPage : ContentPage
    {
        public MessengerPage()
        {
            InitializeComponent();

            ViewModel = Ioc.Default.GetRequiredService<MessengerPageViewModel>();

            BindingContext = ViewModel;
        }

        public MessengerPageViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadDocsCommand.Execute("Messenger");
        }
    }
}