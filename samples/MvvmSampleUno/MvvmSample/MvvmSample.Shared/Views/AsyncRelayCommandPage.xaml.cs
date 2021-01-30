﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace MvvmSample.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AsyncRelayCommandPage : Page
    {
        public AsyncRelayCommandPage()
        {
            this.InitializeComponent();
        }
    }

    // TODO: replace this with the one from the toolkit, when https://github.com/windows-toolkit/WindowsCommunityToolkit/pull/3410 is merged
    public sealed class TaskResultConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //if (value is Task task &&
            //    task.IsCompletedSuccessfully)
            //{
            //    return task.GetType().GetProperty(nameof(Task<object>.Result))?.GetValue(task);
            //}

            return null;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class MarkdownSectionConverter : IValueConverter
    {
        //public int MaxLength { get; set; } = 5;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is IReadOnlyDictionary<string, string> texts)
            {
                var result = texts != null && texts.TryGetValue(parameter as string, out var match) ? match : string.Empty;
                return result;//?.Substring(0, MaxLength);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
