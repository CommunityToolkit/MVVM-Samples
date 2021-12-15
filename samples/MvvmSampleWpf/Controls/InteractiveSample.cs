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

namespace MvvmSampleWpf.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MvvmSampleWpf.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MvvmSampleWpf.Controls;assembly=MvvmSampleWpf.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:InteractiveSample/>
    ///
    /// </summary>
    public class InteractiveSample : ContentControl
    {
        static InteractiveSample()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InteractiveSample), new FrameworkPropertyMetadata(typeof(InteractiveSample)));
        }

        /// <summary>
        /// Gets or sets the <see cref="string"/> representing the C# code to display.
        /// </summary>
        public string CSharpCode
        {
            get => (string)GetValue(CSharpCodeProperty);
            set => SetValue(CSharpCodeProperty, $"```csharp\n{value.Trim()}\n```");
        }

        /// <summary>
        /// The <see cref="DependencyProperty"/> backing <see cref="CSharpCode"/>.
        /// </summary>
        public static readonly DependencyProperty CSharpCodeProperty = DependencyProperty.Register(
            nameof(CSharpCode),
            typeof(string),
            typeof(InteractiveSample),
            new PropertyMetadata(default(string)));

        /// <summary>
        /// Gets or sets the <see cref="string"/> representing the XAML code to display.
        /// </summary>
        public string XamlCode
        {
            get => (string)GetValue(XamlCodeProperty);
            set => SetValue(XamlCodeProperty, $"```xml\n{value.Trim()}\n```");
        }

        /// <summary>
        /// The <see cref="DependencyProperty"/> backing <see cref="CSharpCode"/>.
        /// </summary>
        public static readonly DependencyProperty XamlCodeProperty = DependencyProperty.Register(
            nameof(XamlCode),
            typeof(string),
            typeof(InteractiveSample),
            new PropertyMetadata(default(string)));
    }
}
