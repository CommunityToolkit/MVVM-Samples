using System;
using Windows.UI.Xaml;

namespace MvvmSampleUwp;
public static class ViewModelLocator
{
    private static Func<object, object>? _viewModelFactory;

    public static readonly DependencyProperty InitViewModelProperty =
        DependencyProperty.RegisterAttached("InitViewModel", typeof(bool?), typeof(ViewModelLocator), new PropertyMetadata(null, OnInitViewModelChanged));

    public static bool? GetInitViewModel(DependencyObject dependencyObject)
    {
        return (bool?)dependencyObject.GetValue(InitViewModelProperty);
    }

    public static void SetInitViewModel(DependencyObject dependencyObject, bool? value)
    {
        dependencyObject.SetValue(InitViewModelProperty, value);
    }

    private static void OnInitViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var needSet = (bool?)e.NewValue;
        if (needSet == true && d is FrameworkElement element)
        {
            var viewModel = _viewModelFactory?.Invoke(d);
            element.DataContext = viewModel;
        }
    }

    public static void SetViewModelFactory(Func<object, object> factory)
        => _viewModelFactory = factory;
}
