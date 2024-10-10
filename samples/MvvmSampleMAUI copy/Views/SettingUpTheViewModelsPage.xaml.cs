using MvvmSample.Core.ViewModels;

namespace MvvmSampleMAUI.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SettingUpTheViewModelsPage : BaseContentPage<SamplePageViewModel>
{
    public SettingUpTheViewModelsPage(SamplePageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        BindingContext.LoadDocsCommand.Execute("PuttingThingsTogether");
    }
}