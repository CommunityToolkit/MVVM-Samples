// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using MvvmSample.Core.Services;

namespace MvvmSample.Core.ViewModels;

public class IocPageViewModel : SamplePageViewModel
{
    public IocPageViewModel(IFilesService filesService) 
        : base(filesService)
    {
    }
}
