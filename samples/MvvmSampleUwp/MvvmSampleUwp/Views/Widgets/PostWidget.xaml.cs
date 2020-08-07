using Windows.UI.Xaml.Controls;

namespace MvvmSampleUwp.Views.Widgets
{
    public sealed partial class PostWidget : UserControl
    {
        public PostWidget()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => ViewModel.IsActive = true;
            this.Unloaded += (s, e) => ViewModel.IsActive = false;
        }
    }
}
