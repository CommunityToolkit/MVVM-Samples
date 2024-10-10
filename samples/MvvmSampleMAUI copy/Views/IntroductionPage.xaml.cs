using MvvmSample.Core.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace MvvmSampleMAUI.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class IntroductionPage : BaseContentPage<ObservableObjectPageViewModel>
{
    public IntroductionPage(ObservableObjectPageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        BindingContext.LoadDocsCommand.Execute("Introduction");
    }
}