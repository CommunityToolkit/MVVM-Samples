using System;
using System.Reflection;

namespace MvvmSampleMAUI.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class RedditBrowserPage : ContentPage
{
    public RedditBrowserPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SubredditWidget.PostSelected += HandleSubredditWidgetPostSelected;
        PostWidget.OnAppearing();
        SubredditWidget.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        SubredditWidget.PostSelected -= HandleSubredditWidgetPostSelected;
        PostWidget.OnDisappearing();
    }

    void HandleSubredditWidgetPostSelected(object sender, EventArgs e)
    {
        throw new NotImplementedException();

        //Ugly workaround for https://github.com/xamarin/XamarinCommunityToolkit/issues/595
        // MethodInfo dynMethod = RedditTabView.GetType().GetMethod("UpdateSelectedIndex", BindingFlags.NonPublic | BindingFlags.Instance);
        // dynMethod?.Invoke(RedditTabView, [1, false]);
    }

}