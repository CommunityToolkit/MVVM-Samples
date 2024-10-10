using MvvmSample.Core.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace MvvmSampleMAUI.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MessengerPage : BaseContentPage<MessengerPageViewModel>
{
    public MessengerPage(MessengerPageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        BindingContext.LoadDocsCommand.Execute("Messenger");
    }
}