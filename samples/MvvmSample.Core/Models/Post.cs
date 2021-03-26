// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace MvvmSample.Core.Models
{
    /// <summary>
    /// A class for a query for posts in a given subreddit.
    /// </summary>
    public sealed class PostsQueryResponse
    {
        /// <summary>
        /// Gets or sets the listing data for the response.
        /// </summary>
        [JsonPropertyName("data")]
        public PostListing? Data { get; set; }
    }

    /// <summary>
    /// A class for a Reddit listing of posts.
    /// </summary>
    public sealed class PostListing
    {
        /// <summary>
        /// Gets or sets the items in this listing.
        /// </summary>
        [JsonPropertyName("children")]
        public IList<PostData>? Items { get; set; }
    }

    /// <summary>
    /// A wrapping class for a post.
    /// </summary>
    public sealed class PostData
    {
        /// <summary>
        /// Gets or sets the <see cref="Post"/> instance.
        /// </summary>
        [JsonPropertyName("data")]
        public Post? Data { get; set; }
    }

    /// <summary>
    /// A simple model for a Reddit post.
    /// </summary>
    public sealed class Post
    {
        /// <summary>
        /// Gets or sets the title of the post.
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the URL to the post thumbnail, if present.
        /// </summary>
        [JsonPropertyName("thumbnail")]
        public string? Thumbnail { get; set; }

        /// <summary>
        /// Gets the text of the post.
        /// </summary>
        /// <remarks>
        /// Here we're just hardcoding some sample text to simplify how posts are displayed.
        /// Normally, not all posts have a self text post available.
        /// </remarks>
        [JsonIgnore]
        public string SelfText { get; } = string.Join(" ", Enumerable.Repeat(
@"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", 20));
    }
}
