// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MvvmSample.Core.Models;

/// <summary>
/// A class for a query for contacts.
/// </summary>
/// <param name="Contacts">Gets the list of returned contacts.</param>
public sealed record ContactsQueryResponse([property: JsonPropertyName("results")] IList<Contact> Contacts);

/// <summary>
/// A simple model for a contact.
/// </summary>
/// <param name="Name">Gets the name of the contact.</param>
/// <param name="Email">Gets the email of the contact.</param>
/// <param name="Picture">Gets the picture of the contact.</param>
public sealed record Contact(
    [property: JsonPropertyName("name")] Name Name,
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("picture")] Picture Picture);

/// <summary>
/// A simple model for the name of a contact.
/// </summary>
/// <param name="First">The first name of the contact.</param>
/// <param name="Last">The last name of the contact.</param>
public sealed record Name(
    [property: JsonPropertyName("first")] string First,
    [property: JsonPropertyName("last")] string Last)
{
    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{First} {Last}";
    }
}

/// <summary>
/// A simple model for the picture of a contact.
/// </summary>
/// <param name="Url">The URL of the picture.</param>
public sealed record Picture([property: JsonPropertyName("thumbnail")] string Url);
