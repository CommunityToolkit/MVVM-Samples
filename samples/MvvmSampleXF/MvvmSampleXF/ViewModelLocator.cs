using System;
using Xamarin.Forms;

namespace MvvmSampleXF
{
    public static class ViewModelLocator
    {
        private static Func<object, object> _viewModelFactory;

        public static readonly BindableProperty InitViewModelProperty =
            BindableProperty.CreateAttached("InitViewModel", typeof(bool?), typeof(ViewModelLocator), null, propertyChanged: OnInitViewModelChanged);

        public static bool? GetInitViewModel(BindableObject bindable)
        {
            return (bool?)bindable.GetValue(InitViewModelProperty);
        }

        public static void SetInitViewModel(BindableObject bindable, bool? value)
        {
            bindable.SetValue(InitViewModelProperty, value);
        }

        private static void OnInitViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            bool? needSet = (bool?)newValue;
            if (needSet.HasValue && needSet.Value)
            {
                var viewModel = _viewModelFactory(bindable);
                bindable.BindingContext = viewModel;
            }
        }

        public static void SetViewModelFactory(Func<object, object> factory) 
            => _viewModelFactory = factory;

        
    }
}
