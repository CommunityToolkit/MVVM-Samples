﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using MvvmSampleUwp.Models;

namespace MvvmSampleUwp.ViewModels.Widgets
{
    /// <summary>
    /// A viewmodel for a post widget.
    /// </summary>
    public sealed class PostWidgetViewModel : ObservableRecipient, IRecipient<PropertyChangedMessage<Post>>
    {
        private Post post;

        /// <summary>
        /// Gets the currently selected post, if any.
        /// </summary>
        public Post Post
        {
            get => post;
            private set => SetProperty(ref post, value);
        }

        /// <inheritdoc/>
        public void Receive(PropertyChangedMessage<Post> message)
        {
            if (message.Sender.GetType() == typeof(SubredditWidgetViewModel) &&
                message.PropertyName == nameof(SubredditWidgetViewModel.SelectedPost))
            {
                Post = message.NewValue;
            }
        }
    }
}
