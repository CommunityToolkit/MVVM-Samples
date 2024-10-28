using MvvmSample.Core.ViewModels.Widgets;

namespace MvvmSampleMAUI.Views.Widgets;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SubredditWidget : BaseContentView<SubredditWidgetViewModel>
{
    readonly WeakEventManager postSelectedEventManager = new();

    public SubredditWidget(SubredditWidgetViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();

        Loaded += HandleLoaded;
    }

    public event EventHandler PostSelected
    {
        add => postSelectedEventManager.AddEventHandler(value);
        remove => postSelectedEventManager.RemoveEventHandler(value);
    }

    void CollectionView_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        ArgumentNullException.ThrowIfNull(sender);

        var collectionView = (CollectionView)sender;

        OnPostSelected();

        collectionView.SelectedItem = null;
    }

    void HandleLoaded(object? sender, EventArgs e)
    {
        BindingContext.LoadPostsCommand.Execute(null);
    }

    void OnPostSelected() => postSelectedEventManager.HandleEvent(this, EventArgs.Empty, nameof(PostSelected));
}