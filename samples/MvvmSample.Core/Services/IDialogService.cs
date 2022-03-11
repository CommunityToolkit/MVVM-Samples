// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

namespace MvvmSample.Core.Services;

/// <summary>
/// The default <see langword="interface"/> for a service that shows dialogs
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Shows a message dialog with a title and custom content.
    /// </summary>
    /// <param name="title">The title of the message dialog.</param>
    /// <param name="message">The content of the message dialog.</param>
    Task ShowMessageDialogAsync(string title, string message);
}
