using MvvmSample.Core.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace MvvmSampleMAUI.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class IoCPage : BaseContentPage<IocPageViewModel>
{
    public IoCPage(IocPageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }

    public IocPageViewModel ViewModel => (IocPageViewModel)BindingContext;

    protected override void OnAppearing()
    {
        base.OnAppearing();

        ViewModel.LoadDocsCommand.Execute("IoC");
    }
}