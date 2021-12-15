using System.Windows;

namespace MvvmSampleWpf
{
    public interface IViewFactory
    {
        FrameworkElement? ResolveView(object viewModel);
    }
}
