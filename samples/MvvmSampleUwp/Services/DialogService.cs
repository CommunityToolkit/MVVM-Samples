// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmSample.Core.Services;
using Windows.UI.Xaml.Controls;

#nullable enable

namespace MvvmSampleUwp.Services;

/// <summary>
/// A <see langword="class"/> that implements the <see cref="IDialogService"/> <see langword="interface"/> using UWP APIs.
/// </summary>
public sealed class DialogService : IDialogService
{
    /// <inheritdoc/>
    public Task ShowMessageDialogAsync(string title, string message)
    {
        ContentDialog dialog = new();
        dialog.Title = title;
        dialog.CloseButtonText = "Close";
        dialog.DefaultButton = ContentDialogButton.Close;
        dialog.Content = message;

        return dialog.ShowAsync().AsTask();
    }
}
