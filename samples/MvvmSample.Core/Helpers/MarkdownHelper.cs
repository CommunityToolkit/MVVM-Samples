// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MvvmSample.Core.Helpers;

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
           Regex.Matches(text, @"(?<=\W)#+ ([^\n]+).+?(?=\W#|$)", RegexOptions.Singleline)
           .OfType<Match>()
            .ToDictionary(
                m => Regex.Replace(m.Groups[1].Value.Trim().Replace("&lt;", "<"), @"\[([^]]+)\]\([^)]+\)", m => m.Groups[1].Value),
                m => m.Groups[0].Value.Trim().Replace("&lt;", "<").Replace("[!WARNING]", "**WARNING:**").Replace("[!NOTE]", "**NOTE:**"));
    }
}
