using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmSampleXF.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InteractiveSample : ContentView
    {
        public static readonly BindableProperty CSharpCodeProperty = BindableProperty.Create(nameof(CSharpCode), typeof(string), typeof(InteractiveSample), string.Empty);
        public static readonly BindableProperty XamlCodeProperty = BindableProperty.Create(nameof(XamlCode), typeof(string), typeof(InteractiveSample), string.Empty);

        public string CSharpCode
        {
            get => (string)GetValue(CSharpCodeProperty);
            set => SetValue(CSharpCodeProperty, value);
        }

        public string XamlCode
        {
            get => (string)GetValue(XamlCodeProperty);
            set => SetValue(XamlCodeProperty, value);
        }
        public InteractiveSample()
        {
            InitializeComponent();
        }
    }
}