// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace MvvmSample.Core.Models;

/// <summary>
/// A class for a query for posts in a given subreddit.
/// </summary>
/// <param name="Data">Gets the listing data for the response.</param>
public sealed record PostsQueryResponse([property: JsonPropertyName("data")] PostListing Data);

/// <summary>
/// A class for a Reddit listing of posts.
/// </summary>
/// <param name="Items">Gets the items in this listing,</param>
public sealed record PostListing([property: JsonPropertyName("children")] IList<PostData> Items);

/// <summary>
/// A wrapping class for a post.
/// </summary>
/// <param name="Data">Gets the <see cref="Post"/> instance.</param>
public sealed record PostData([property: JsonPropertyName("data")] Post Data);

/// <summary>
/// A simple model for a Reddit post.
/// </summary>
/// <param name="Title">Gets the title of the post.</param>
/// <param name="Thumbnail">Gets the URL to the post thumbnail, if present.</param>
public sealed record Post(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("thumbnail")] string? Thumbnail)
{
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
