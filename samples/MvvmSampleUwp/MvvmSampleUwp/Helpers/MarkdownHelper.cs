using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MvvmSampleUwp.Helpers
{
    /// <summary>
    /// A simple class to help with basic operations on markdown documents.
    /// </summary>
    public static class MarkdownHelper
    {
        /// <summary>
        /// Gets all the paragraphs in a given markdown document.
        /// </summary>
        /// <param name="text">The input markdown document.</param>
        /// <returns>The raw paragraphs from <paramref name="text"/>.</returns>
        public static IReadOnlyDictionary<string, string> GetParagraphs(string text)
        {
            return
                Regex.Matches(text, @"(#+ ([^\n]+)[^#]+)", RegexOptions.Singleline)
                .ToDictionary(
                    m => m.Groups[2].Value.Trim(),
                    m => m.Groups[1].Value.Trim());
        }
    }
}
