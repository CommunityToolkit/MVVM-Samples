using MvvmSample.Core.ViewModels.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MvvmSample.Core.ViewModels.Widgets.Messages;

namespace MvvmSampleXF.Views.Widgets
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostMessageWidget : ContentView
    {
        public PostMessageWidget()
        {
            InitializeComponent();

            ViewModel = Ioc.Default.GetRequiredService<PostWidgetMessageViewModel>();

            BindingContext = ViewModel;
        }

        public PostWidgetMessageViewModel ViewModel { get; }

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