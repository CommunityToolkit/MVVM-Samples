using System.Threading.Tasks;
using MvvmSampleUwp.Models;
using Refit;

namespace MvvmSampleUwp.Services
{
    /// <summary>
    /// An interface for a simple Reddit service.
    /// </summary>
    public interface IRedditService
    {
        /// <summary>
        /// Get a list of posts from a given subreddit
        /// </summary>
        /// <param name="subreddit">The subreddit name.</param>
        [Get("/r/{subreddit}/.json")]
        Task<PostsQueryResponse> GetSubredditPostsAsync(string subreddit);
    }
}
