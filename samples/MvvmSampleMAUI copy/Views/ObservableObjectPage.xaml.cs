using MvvmSample.Core.ViewModels;

namespace MvvmSampleMAUI.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ObservableObjectPage : BaseContentPage<ObservableObjectPageViewModel>
{
    public ObservableObjectPage(ObservableObjectPageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        BindingContext.LoadDocsCommand.Execute("ObservableObject");
    }
}