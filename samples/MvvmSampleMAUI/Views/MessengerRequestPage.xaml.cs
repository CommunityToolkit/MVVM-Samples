using MvvmSample.Core.ViewModels;

namespace MvvmSampleMAUI.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MessengerRequestPage : BaseContentPage<MessengerPageViewModel>
{
    public MessengerRequestPage(MessengerPageViewModel viewModel) : base(viewModel)
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

        ViewModel.SenderViewModel.IsActive = false;
        ViewModel.ReceiverViewModel.IsActive = false;
    }
}