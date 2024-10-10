using MvvmSample.Core.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
namespace MvvmSampleMAUI.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SettingsServicePage : BaseContentPage<SamplePageViewModel>
{
    public SettingsServicePage(SamplePageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        BindingContext.LoadDocsCommand.Execute("PuttingThingsTogether");
    }
}