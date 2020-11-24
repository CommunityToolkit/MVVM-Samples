using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmSampleXF.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RedditServicePage : ContentPage
	{
		public RedditServicePage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			ViewModel.LoadDocsCommand.Execute("PuttingThingsTogether");
		}
	}
}