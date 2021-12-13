using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MvvmSample.Core.ViewModels.Widgets.NoMessages;
using MvvmSample.Core.ViewModels;

namespace MvvmSampleXF.Views.Widgets
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostWidget : ContentView
    {
        public PostWidget()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is RedditBrowserPageViewModel)
                BindingContext = (BindingContext as RedditBrowserPageViewModel)?.PostWidgetViewModel;
        }

        public PostWidgetViewModel ViewModel => BindingContext as PostWidgetViewModel;
    }
}