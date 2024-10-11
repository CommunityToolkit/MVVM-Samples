using MvvmSample.Core.ViewModels;

namespace MvvmSampleMAUI.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MessengerSendPage : BaseContentPage<MessengerPageViewModel>
{
    public MessengerSendPage(MessengerPageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        BindingContext.LoadDocsCommand.Execute("Messenger");
        BindingContext.SenderViewModel.IsActive = true;
        BindingContext.ReceiverViewModel.IsActive = true;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        BindingContext.SenderViewModel.IsActive = false;
        BindingContext.ReceiverViewModel.IsActive = false;
    }
}