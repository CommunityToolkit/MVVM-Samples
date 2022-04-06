// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmSample.Core.Models;
using Refit;

namespace MvvmSample.Core.Services;

/// <summary>
/// An interface for a simple contacts service.
/// </summary>
public interface IContactsService
{
    /// <summary>
    /// Get a list of contacts.
    /// </summary>
    /// <param name="count">The number of contacts to retrieve.</param>
    [Get("/api/?dataType=json&inc=name,email,picture")]
    Task<ContactsQueryResponse> GetContactsAsync([AliasAs("results")] int count);
}
