﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MvvmSampleUwp.Controls
{
    /// <summary>
    /// A simple control that acts as a frame for an interactive sample.
    /// </summary>
    public sealed class InteractiveSample : ContentControl
    {
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
