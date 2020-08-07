using System.Collections.Generic;
using Newtonsoft.Json;

namespace MvvmSampleUwp.Models
{
    /// <summary>
    /// A class for a query for posts in a given subreddit.
    /// </summary>
    public class PostsQueryResponse
    {
        /// <summary>
        /// Gets or sets the listing data for the response.
        /// </summary>
        [JsonProperty("data")]
        public PostListing Data { get; set; }
    }

    /// <summary>
    /// A class for a Reddit listing of posts.
    /// </summary>
    public class PostListing
    {
        /// <summary>
        /// Gets or sets the items in this listing.
        /// </summary>
        [JsonProperty("children")]
        public IList<PostData> Items { get; set; }
    }

    /// <summary>
    /// A wrapping class for a post.
    /// </summary>
    public class PostData
    {
        /// <summary>
        /// Gets or sets the <see cref="Post"/> instance.
        /// </summary>
        [JsonProperty("data")]
        public Post Data { get; set; }
    }

    /// <summary>
    /// A simple model for a Reddit post.
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Gets or sets the title of the post.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL to the post thumbnail, if present.
        /// </summary>
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }
    }
}
