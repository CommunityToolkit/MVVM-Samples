using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MvvmSampleWpf
{
    public class MappingViewFactory : IViewFactory
    {
        private readonly Dictionary<Type, Type> _mapping = new();

        public FrameworkElement? ResolveView(object viewModel)
        {
            if (!_mapping.ContainsKey(viewModel.GetType()))
            {
                return null;
            }

            var viewType = _mapping[viewModel.GetType()];
            return (FrameworkElement?)Activator.CreateInstance(viewType);
        }

        public MappingViewFactory Register<TView, TViewModel>()
            where TView : DependencyObject
            where TViewModel : ObservableObject
        {
            _mapping[typeof(TViewModel)] = typeof(TView);

            return this;
        }

    }
}
