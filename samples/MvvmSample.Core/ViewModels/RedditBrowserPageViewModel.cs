using CommunityToolkit.Mvvm.ComponentModel;
using MvvmSample.Core.ViewModels.Widgets.NoMessages;
using System;
using System.ComponentModel;
using System.Reactive.Linq;

namespace MvvmSample.Core.ViewModels
{
    public class RedditBrowserPageViewModel : ObservableObject
    {
        public RedditBrowserPageViewModel(PostWidgetViewModel postWidgetViewModel, SubredditWidgetViewModel subredditWidgetViewModel)
        {
            PostWidgetViewModel = postWidgetViewModel;
            SubredditWidgetViewModel = subredditWidgetViewModel;

            var subscription = Observable.FromEventPattern<PropertyChangedEventArgs>(SubredditWidgetViewModel, nameof(INotifyPropertyChanged.PropertyChanged))
                .Where(x => x.EventArgs.PropertyName == nameof(SubredditWidgetViewModel.SelectedPost))
                .Subscribe(pattern => PostWidgetViewModel.Post = SubredditWidgetViewModel.SelectedPost);
        }

        public PostWidgetViewModel PostWidgetViewModel { get; }
        public SubredditWidgetViewModel SubredditWidgetViewModel { get; }
        
    }
}
