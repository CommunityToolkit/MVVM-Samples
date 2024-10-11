using MvvmSample.Core.ViewModels.Widgets;

namespace MvvmSampleMAUI.Views.Widgets;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class PostWidget : BaseContentView<PostWidgetViewModel>
{
    public PostWidget(PostWidgetViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();

        Loaded += HandleLoaded;
        Unloaded += HandleUnloaded;
    }

    void HandleLoaded(object? sender, EventArgs e)
    {
        BindingContext.IsActive = true;
    }

    void HandleUnloaded(object? sender, EventArgs e)
    {
        BindingContext.IsActive = false;
    }
}