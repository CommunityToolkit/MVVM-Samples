using System;
using Windows.UI.Xaml;

namespace MvvmSampleUwp
{
    public static class ViewModelLocator
    {
        private static Func<object, object> _viewModelFactory;

        public static readonly DependencyProperty InitViewModelProperty =
            DependencyProperty.RegisterAttached("InitViewModel", typeof(bool?), typeof(ViewModelLocator), new PropertyMetadata(null, OnInitViewModelChanged));

        public static bool? GetInitViewModel(DependencyObject bindable)
        {
            return (bool?)bindable.GetValue(InitViewModelProperty);
        }

        public static void SetInitViewModel(DependencyObject bindable, bool? value)
        {
            bindable.SetValue(InitViewModelProperty, value);
        }

        private static void OnInitViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool? needSet = (bool?)e.NewValue;
            if (needSet.HasValue && needSet.Value && d is FrameworkElement element)
            {
                var viewModel = _viewModelFactory(d);
                element.DataContext = viewModel;
            }
        }

        public static void SetViewModelFactory(Func<object, object> factory) 
            => _viewModelFactory = factory;

        
    }
}
