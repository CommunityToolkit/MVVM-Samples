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
    public partial class SubredditMessageWidget : ContentView
    {
        public event EventHandler PostSelected;
        public SubredditMessageWidget()
        {
            InitializeComponent();

            ViewModel = Ioc.Default.GetRequiredService<SubredditWidgetMessageViewModel>();

            BindingContext = ViewModel;
        }

        public SubredditWidgetMessageViewModel ViewModel { get; }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PostSelected?.Invoke(this, EventArgs.Empty);
        }

        public void OnAppearing()
        {
            ViewModel.LoadPostsCommand.Execute(null);
        }
    }
}