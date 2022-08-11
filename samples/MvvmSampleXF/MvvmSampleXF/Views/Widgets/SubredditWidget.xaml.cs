using CommunityToolkit.Mvvm.DependencyInjection;
using MvvmSample.Core.ViewModels.Widgets;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmSampleXF.Views.Widgets
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubredditWidget : ContentView
    {
        public event EventHandler? PostSelected;

        public SubredditWidget()
        {
            InitializeComponent();

            BindingContext = Ioc.Default.GetRequiredService<SubredditWidgetViewModel>();
        }

        public SubredditWidgetViewModel ViewModel => (SubredditWidgetViewModel)BindingContext;

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