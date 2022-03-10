// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmSample.Core.Services;

/// <summary>
/// The default <see langword="interface"/> for the settings manager used in the app.
/// </summary>
public interface ISettingsService
{
    /// <summary>
    /// Assigns a value to a settings key.
    /// </summary>
    /// <typeparam name="T">The type of the object bound to the key.</typeparam>
    /// <param name="key">The key to check.</param>
    /// <param name="value">The value to assign to the setting key.</param>
    void SetValue<T>(string key, T? value);

    /// <summary>
    /// Reads a value from the current <see cref="IServiceProvider"/> instance and returns its casting in the right type.
    /// </summary>
    /// <typeparam name="T">The type of the object to retrieve.</typeparam>
    /// <param name="key">The key associated to the requested object.</param>
    T? GetValue<T>(string key);
}
