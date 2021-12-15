using CommunityToolkit.Mvvm.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace MvvmSampleWpf
{
    public class ViewModelPresenter : ContentControl
    {
        public ViewModelPresenter()
        {
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(object), typeof(ViewModelPresenter), new PropertyMetadata(null, OnViewModelChanged));

        public object ViewModel
        {
            get { return GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private static void OnViewModelChanged(DependencyObject changedObject, DependencyPropertyChangedEventArgs args)
        {
            var contentControl = (ViewModelPresenter)changedObject;
            contentControl.RefreshContentPresenter();
        }

        private void RefreshContentPresenter()
        {
            if (ViewModel == null)
            {
                Content = null;

                return;
            }

            var viewFactory = Ioc.Default.GetRequiredService<IViewFactory>();
            var view = viewFactory.ResolveView(ViewModel);

            if (view != null)
            {
                view.DataContext = ViewModel;
                Content = view;
            }
            else
            {
                Content = null;
            }
        }
    }
}
