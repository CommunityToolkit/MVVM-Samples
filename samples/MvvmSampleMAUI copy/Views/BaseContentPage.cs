using CommunityToolkit.Mvvm.ComponentModel;
namespace MvvmSampleMAUI.Views;

public abstract class BaseContentPage<TViewModel> : ContentPage where TViewModel : ObservableObject
{
	protected BaseContentPage(TViewModel viewModel)
	{
		base.BindingContext = viewModel;
	}

	protected new TViewModel BindingContext => (TViewModel)base.BindingContext;
}