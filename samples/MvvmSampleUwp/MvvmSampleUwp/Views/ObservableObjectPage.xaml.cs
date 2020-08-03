using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MvvmSampleUwp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ObservableObjectPage : Page
    {
        public ObservableObjectPage()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.LoadDocsCommand.Execute("ObservableObject");
        }
    }
}
