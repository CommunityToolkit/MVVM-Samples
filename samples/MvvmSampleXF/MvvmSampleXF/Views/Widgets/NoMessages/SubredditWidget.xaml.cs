using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MvvmSample.Core.ViewModels.Widgets.NoMessages;
using MvvmSample.Core.ViewModels;

namespace MvvmSampleXF.Views.Widgets
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubredditWidget : ContentView
    {
        public event EventHandler PostSelected;
        public SubredditWidget()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is RedditBrowserPageViewModel)
                BindingContext = (BindingContext as RedditBrowserPageViewModel)?.SubredditWidgetViewModel;
        }

        public SubredditWidgetViewModel ViewModel => BindingContext as SubredditWidgetViewModel;

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