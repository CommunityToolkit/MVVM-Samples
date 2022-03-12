using System;
using Xamarin.Forms;

namespace MvvmSampleXF;
public static class ViewModelLocator
{
    private static Func<object, object>? _viewModelFactory;

    public static readonly BindableProperty InitViewModelProperty =
        BindableProperty.CreateAttached("InitViewModel", typeof(bool?), typeof(ViewModelLocator), null, propertyChanged: OnInitViewModelChanged);

    public static bool? GetInitViewModel(BindableObject bindableObject)
    {
        return (bool?)bindableObject.GetValue(InitViewModelProperty);
    }

    public static void SetInitViewModel(BindableObject bindableObject, bool? value)
    {
        bindableObject.SetValue(InitViewModelProperty, value);
    }

    private static void OnInitViewModelChanged(BindableObject bindableObject, object oldValue, object newValue)
    {
        var needSet = (bool?)newValue;
        if (needSet == true)
        {
            var viewModel = _viewModelFactory?.Invoke(bindableObject);
            bindableObject.BindingContext = viewModel;
        }
    }

    public static void SetViewModelFactory(Func<object, object> factory)
        => _viewModelFactory = factory;
}