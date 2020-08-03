using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MvvmSampleUwp
{
    public sealed partial class Shell : UserControl
    {
        public Shell()
        {
            this.InitializeComponent();

            // Set the custom title bar to act as a draggable region
            Window.Current.SetTitleBar(TitleBarBorder);
        }
    }
}
