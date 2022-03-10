// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

#nullable enable

namespace Microsoft.Toolkit.Uwp.UI.Converters;

/// <summary>
/// Custom version of the converter from the Toolkit, with a bug fix.
/// This should be moved over to the Toolkit in the next release.
/// </summary>
public sealed class TaskResultConverter2 : IValueConverter
{
    /// <inheritdoc/>
    public object? Convert(object? value, Type? targetType, object? parameter, string? language)
    {
        if (value is Task task)
        {
            return task.GetResultOrDefault();
        }

        return null;
    }

    /// <inheritdoc/>
    public object ConvertBack(object? value, Type? targetType, object? parameter, string? language)
    {
        throw new NotImplementedException();
    }
}
