using CommunityToolkit.Mvvm.ComponentModel;
namespace MvvmSampleMAUI.Views.Widgets;

public abstract class BaseContentView<TViewModel> : ContentView where TViewModel : ObservableObject
{
    protected BaseContentView(TViewModel viewModel)
    {
        base.BindingContext = viewModel;
    }

    protected new TViewModel BindingContext => (TViewModel)base.BindingContext;
}