using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public class NamingConvenionViewFactory : IViewFactory
    {
        public FrameworkElement? ResolveView(object viewModel)
        {
            var vmName = viewModel.GetType().Name;
            var viewName = vmName.Contains("Page") ? vmName.Replace("PageViewModel", "View") : vmName.Replace("ViewModel", "");
            var viewType = typeof(App).Assembly.DefinedTypes.Where(x => x.Name == viewName).FirstOrDefault();
            if (viewType == null) return null;

            var view = Activator.CreateInstance(viewType);
            return (FrameworkElement?)view;
        }        
    }
}
