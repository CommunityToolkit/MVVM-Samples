using CommunityToolkit.Mvvm.DependencyInjection;
using MvvmSample.Core.ViewModels.Widgets.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MvvmSampleWpf.Views.Widgets.Messages
{
    /// <summary>
    /// Interaction logic for SubredditMessageWidget.xaml
    /// </summary>
    public partial class SubredditMessageWidget : UserControl
    {
        public SubredditMessageWidget()
        {
            InitializeComponent();

            ViewModel = Ioc.Default.GetRequiredService<SubredditWidgetMessageViewModel>();

            DataContext = ViewModel;
        }

        public SubredditWidgetMessageViewModel ViewModel { get; }
    }
}
