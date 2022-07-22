// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

#nullable enable

namespace MvvmSampleUwp.Controls;

/// <summary>
/// A simple control that acts as a container for a documentation block.
/// </summary>
[TemplatePart(Name = "PART_MarkdownTextBlock", Type = typeof(MarkdownTextBlock))]
public sealed class DocumentationBlock : ContentControl
{
    /// <summary>
    /// Creates a new <see cref="DocumentationBlock"/> instance.
    /// </summary>
    public DocumentationBlock()
    {
        Implicit.SetAnimations(this, new ImplicitAnimationSet { new OffsetAnimation() });
    }

    /// <summary>
    /// The <see cref="MarkdownTextBlock"/> instance in use.
    /// </summary>
    private MarkdownTextBlock? markdownTextBlock;

    /// <inheritdoc/>
    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        markdownTextBlock = (MarkdownTextBlock)GetTemplateChild("PART_MarkdownTextBlock")!;

        markdownTextBlock.LinkClicked += MarkdownTextBlock_LinkClicked;
        markdownTextBlock.ImageResolving += MarkdownTextBlock_ImageResolving;
    }

    /// <summary>
    /// Handles a clicked link in a markdown block.
    /// </summary>
    /// <param name="sender">The source <see cref="MarkdownTextBlock"/> control.</param>
    /// <param name="e">The input arguments.</param>
    private void MarkdownTextBlock_LinkClicked(object sender, LinkClickedEventArgs e)
    {
        if (Uri.TryCreate(e.Link, UriKind.Absolute, out Uri result) ||
            (e.Link.StartsWith("/dotnet") &&
             Uri.TryCreate($"https://docs.microsoft.com{e.Link}", UriKind.Absolute, out result)))
        {
            _ = Launcher.LaunchUriAsync(result);
        }
    }

    /// <summary>
    /// Handles an image being resolved and loads images from the local assets.
    /// </summary>
    /// <param name="sender">The source <see cref="MarkdownTextBlock"/> control.</param>
    /// <param name="e">The input arguments.</param>
    private void MarkdownTextBlock_ImageResolving(object sender, ImageResolvingEventArgs e)
    {
        if (e.Url?.Contains("source-generator-visualization.png") == true)
        {
            e.Image = new BitmapImage(new Uri("ms-appx:///Assets/docs/images/source-generator-visualization.png"));
            e.Handled = true;
        }
    }

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
        typeof(DocumentationBlock),
        new PropertyMetadata(default(string)));
}
