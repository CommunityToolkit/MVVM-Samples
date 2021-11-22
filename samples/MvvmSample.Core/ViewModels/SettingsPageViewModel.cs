// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using MvvmSample.Core.Services;

namespace MvvmSample.Core.ViewModels
{
    /// <summary>
    /// The ViewModel for the SettingsPage.
    /// </summary>
    public class SettingsPageViewModel : ObservableObject
    {
        /// <summary>
        /// Gets the <see cref="ISettingsService"/> instance to use.
        /// </summary>
        private readonly ISettingsService SettingsService = Ioc.Default.GetRequiredService<ISettingsService>();


        public SettingsPageViewModel()
        {

        }

        /// <summary>
        /// A sample boolean settingsvalue bound to the UI
        /// </summary>
        public bool BoolSetting
        {
            get => SettingsService.BoolSampleSetting;
            set => SettingsService.BoolSampleSetting = value;
        }

        /// <summary>
        /// A sample boolean settingsvalue bound to the UI
        /// </summary>
        public bool BoolSetting2
        {
            get => SettingsService.BoolSampleSetting2;
            set => SettingsService.BoolSampleSetting2 = value;
        }

    }
}

