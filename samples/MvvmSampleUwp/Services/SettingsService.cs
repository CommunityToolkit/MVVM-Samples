// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using MvvmSample.Core.Services;
using Windows.Foundation.Collections;
using Windows.Storage;

#nullable enable

namespace MvvmSampleUwp.Services;

/// <summary>
/// A simple <see langword="class"/> that handles the local app settings.
/// </summary>
public sealed class SettingsService : ISettingsService
{
    /// <summary>
    /// The <see cref="IPropertySet"/> with the settings targeted by the current instance.
    /// </summary>
    private readonly IPropertySet SettingsStorage = ApplicationData.Current.LocalSettings.Values;

    /// <inheritdoc/>
    public void SetValue<T>(string key, T? value)
    {
        if (!SettingsStorage.ContainsKey(key)) SettingsStorage.Add(key, value);
        else SettingsStorage[key] = value;
    }

    /// <inheritdoc/>
    public T? GetValue<T>(string key)
    {
        if (SettingsStorage.TryGetValue(key, out object? value))
        {
            return (T)value!;
        }

        return default;
    }
}
