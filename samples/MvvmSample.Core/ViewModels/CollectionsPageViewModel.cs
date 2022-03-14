// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Common.Collections;
using MvvmSample.Core.Services;

namespace MvvmSample.Core.ViewModels;

public class CollectionsPageViewModel : SamplePageViewModel
{
    public CollectionsPageViewModel(IFilesService filesService) 
        : base(filesService)
    {
    }

    public ObservableGroupedCollection<string, Person> Contacts { get; } = new()
    {
        new ObservableGroup<string, Person>("A")
        {
            new Person() { FirstName = "Adam", LastName = "Smith" }
        },
        new ObservableGroup<string, Person>("B")
        {
            new Person() { FirstName = "Bob", LastName = "Ross" },
            new Person() { FirstName = "Barbara", LastName = "White" }
        }
    };
}

public sealed class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
