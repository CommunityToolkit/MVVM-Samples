// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmSample.Core.Models;
using MvvmSample.Core.Services;

namespace MvvmSample.Core.ViewModels.Widgets;

/// <summary>
/// A viewmodel for a contacts list widget.
/// </summary>
public partial class ContactsListWidgetViewModel : ObservableObject
{
    /// <summary>
    /// The <see cref="IContactsService"/> instance currently in use.
    /// </summary>
    private readonly IContactsService ContactsService;

    public ContactsListWidgetViewModel(IContactsService contactsService)
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
    [RelayCommand(FlowExceptionsToTaskScheduler = true)]
    private async Task LoadContactsAsync()
    {
        ContactsQueryResponse contacts = await ContactsService.GetContactsAsync(50);

        Contacts = new ObservableGroupedCollection<string, Contact>(
            contacts.Contacts
            .GroupBy(static c => char.ToUpperInvariant(c.Name.First[0]).ToString())
            .OrderBy(static g => g.Key));

        OnPropertyChanged(nameof(Contacts));
    }

    /// <summary>
    /// Loads more contacts.
    /// </summary>
    [RelayCommand(FlowExceptionsToTaskScheduler = true)]
    private async Task LoadMoreContactsAsync()
    {
        ContactsQueryResponse contacts = await ContactsService.GetContactsAsync(10);

        foreach (Contact contact in contacts.Contacts)
        {
            string key = char.ToUpperInvariant(contact.Name.First[0]).ToString();

            Contacts.InsertItem(
                key: key,
                keyComparer: Comparer<string>.Default,
                item: contact,
                itemComparer: Comparer<Contact>.Create(static (left, right) => Comparer<string>.Default.Compare(left.ToString(), right.ToString())));
        }
    }

    /// <summary>
    /// Removes a given contact from the list.
    /// </summary>
    /// <param name="contact">The contact to remove.</param>
    [RelayCommand]
    private void DeleteContact(Contact contact)
    {
        Contacts.FirstGroupByKey(char.ToUpperInvariant(contact.Name.First[0]).ToString()).Remove(contact);
    }
}
