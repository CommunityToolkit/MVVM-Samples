// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using MvvmSample.Core.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

#nullable enable

namespace MvvmSampleUwp.Services;

/// <summary>
/// A <see langword="class"/> that implements the <see cref="IFilesService"/> <see langword="interface"/> using UWP APIs.
/// </summary>
public sealed class FilesService : IFilesService
{
    /// <inheritdoc/>
    public string InstallationPath => Package.Current.InstalledLocation.Path;

    /// <inheritdoc/>
    public async Task<Stream> OpenForReadAsync(string path)
    {
        StorageFile file = await StorageFile.GetFileFromPathAsync(Path.Combine(InstallationPath, path));

        return await file.OpenStreamForReadAsync();
    }
}
