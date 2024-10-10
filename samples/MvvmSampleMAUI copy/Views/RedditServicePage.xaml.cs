using MvvmSample.Core.ViewModels;

namespace MvvmSampleMAUI.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class RedditServicePage : BaseContentPage<SamplePageViewModel>
{
    public RedditServicePage(SamplePageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext.LoadDocsCommand.Execute("PuttingThingsTogether");
    }
}