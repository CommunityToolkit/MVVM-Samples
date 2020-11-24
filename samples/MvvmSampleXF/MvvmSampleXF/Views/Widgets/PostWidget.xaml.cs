using MvvmSample.Core.ViewModels.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmSampleXF.Views.Widgets
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PostWidget : ContentView
	{
		public PostWidget()
		{
			InitializeComponent();
		}

		public void OnAppearing()
		{
			ViewModel.IsActive = true;
		}

		public void OnDisappearing()
		{
			ViewModel.IsActive = false;
		}
	}
}