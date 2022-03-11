// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace MvvmSampleUwp.Controls;

/// <summary>
/// A simple control that acts as a container for a documentation block.
/// </summary>
public sealed class ValidationTextBox : ContentControl
{
    /// <summary>
    /// Gets or sets the <see cref="string"/> representing the text to display.
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// The <see cref="DependencyProperty"/> backing <see cref="Text"/>.
    /// </summary>
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(ValidationTextBox),
        new PropertyMetadata(default(string)));

    /// <summary>
    /// Gets or sets the <see cref="string"/> representing the header text to display.
    /// </summary>
    public string HeaderText
    {
        get => (string)GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }

    /// <summary>
    /// The <see cref="DependencyProperty"/> backing <see cref="HeaderText"/>.
    /// </summary>
    public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register(
        nameof(HeaderText),
        typeof(string),
        typeof(ValidationTextBox),
        new PropertyMetadata(default(string)));

    /// <summary>
    /// Gets or sets the <see cref="string"/> representing the placeholder text to display.
    /// </summary>
    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    /// <summary>
    /// The <see cref="DependencyProperty"/> backing <see cref="PlaceholderText"/>.
    /// </summary>
    public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
        nameof(PlaceholderText),
        typeof(string),
        typeof(ValidationTextBox),
        new PropertyMetadata(default(string)));

    /// <summary>
    /// Gets or sets the <see cref="string"/> representing the input scope to use.
    /// </summary>
    public InputScope InputScope
    {
        get => (InputScope)GetValue(InputScopeProperty);
        set => SetValue(InputScopeProperty, value);
    }

    /// <summary>
    /// The <see cref="DependencyProperty"/> backing <see cref="InputScope"/>.
    /// </summary>
    public static readonly DependencyProperty InputScopeProperty = DependencyProperty.Register(
        nameof(InputScope),
        typeof(InputScope),
        typeof(ValidationTextBox),
        new PropertyMetadata(default(InputScope)));
}
