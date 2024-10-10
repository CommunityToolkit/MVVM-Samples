using MvvmSample.Core.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace MvvmSampleMAUI.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class PuttingThingsTogetherPage : BaseContentPage<SamplePageViewModel>
{
    public PuttingThingsTogetherPage(SamplePageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        BindingContext.LoadDocsCommand.Execute("PuttingThingsTogether");
    }
}