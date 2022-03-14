// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Common.Collections;
using CommunityToolkit.Mvvm.Input;
using MvvmSample.Core.Models;
using MvvmSample.Core.Services;

namespace MvvmSample.Core.ViewModels;

public partial class CollectionsPageViewModel : SamplePageViewModel
{
    /// <summary>
    /// The <see cref="IContactsService"/> instance currently in use.
    /// </summary>
    private readonly IContactsService ContactsService;

    public CollectionsPageViewModel(IFilesService filesService, IContactsService contactsService) 
        : base(filesService)
    {
        ContactsService = contactsService;
    }

    /// <summary>
    /// Gets the current collection of contacts
    /// </summary>
    public ObservableGroupedCollection<string, Contact> Contacts { get; private set; } = new();

    /// <summary>
    /// Loads the contacts to display.
    /// </summary>
    [ICommand]
    private async Task LoadContactsAsync()
    {
        ContactsQueryResponse contacts = await ContactsService.GetContactsAsync(50);

        Contacts = new ObservableGroupedCollection<string, Contact>(
            contacts.Contacts
            .GroupBy(static c => char.ToUpperInvariant(c.Name.First[0]).ToString())
            .OrderBy(static g => g.Key));

        OnPropertyChanged(nameof(Contacts));
    }
}
