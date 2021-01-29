﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmSampleUwp.Models;
using MvvmSampleUwp.Services;
using Nito.AsyncEx;

namespace MvvmSampleUwp.ViewModels.Widgets
{
    /// <summary>
    /// A viewmodel for a subreddit widget.
    /// </summary>
    public sealed class SubredditWidgetViewModel : ObservableRecipient
    {
        /// <summary>
        /// Gets the <see cref="IRedditService"/> instance to use.
        /// </summary>
        private readonly IRedditService RedditService = Ioc.Default.GetRequiredService<IRedditService>();

        /// <summary>
        /// Gets the <see cref="ISettingsService"/> instance to use.
        /// </summary>
        private readonly ISettingsService SettingsService = Ioc.Default.GetRequiredService<ISettingsService>();

        /// <summary>
        /// An <see cref="AsyncLock"/> instance to avoid concurrent requests.
        /// </summary>
        private readonly AsyncLock LoadingLock = new AsyncLock();

        /// <summary>
        /// Creates a new <see cref="SubredditWidgetViewModel"/> instance.
        /// </summary>
        public SubredditWidgetViewModel()
        {
            LoadPostsCommand = new AsyncRelayCommand(LoadPostsAsync);

            selectedSubreddit = SettingsService.GetValue<string>(nameof(SelectedSubreddit)) ?? Subreddits[0];
        }

        /// <summary>
        /// Gets the <see cref="IAsyncRelayCommand"/> instance responsible for loading posts.
        /// </summary>
        public IAsyncRelayCommand LoadPostsCommand { get; }

        /// <summary>
        /// Gets the collection of loaded posts.
        /// </summary>
        public ObservableCollection<Post> Posts { get; } = new ObservableCollection<Post>();

        /// <summary>
        /// Gets the collection of available subreddits to pick from.
        /// </summary>
        public IReadOnlyList<string> Subreddits { get; } = new[]
        {
            "microsoft",
            "windows",
            "surface",
            "windowsphone",
            "dotnet",
            "csharp"
        };

        private string selectedSubreddit;

        /// <summary>
        /// Gets or sets the currently selected subreddit.
        /// </summary>
        public string SelectedSubreddit
        {
            get => selectedSubreddit;
            set
            {
                SetProperty(ref selectedSubreddit, value);

                SettingsService.SetValue(nameof(SelectedSubreddit), value);
            }
        }

        private Post selectedPost;

        /// <summary>
        /// Gets or sets the currently selected subreddit.
        /// </summary>
        public Post SelectedPost
        {
            get => selectedPost;
            set => SetProperty(ref selectedPost, value, true);
        }

        /// <summary>
        /// Loads the posts from a specified subreddit.
        /// </summary>
        private async Task LoadPostsAsync()
        {
            using (await LoadingLock.LockAsync())
            {
                try
                {
                    var response = await RedditService.GetSubredditPostsAsync(SelectedSubreddit);

                    Posts.Clear();

                    foreach (var item in response.Data.Items)
                    {
                        Posts.Add(item.Data);
                    }
                }
                catch
                {
                    // Whoops!
                }
            }
        }
    }
}
