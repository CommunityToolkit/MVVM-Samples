---
title: ObservableGroupedCollections
author: Sergio0694
description: Custom grouped observable collection types to help display grouped items bound to the UI
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, componentmodel, property changed, collection, collection changed, group, grouped, notification, binding, net core, net standard
dev_langs:
  - csharp
---

# ObservableGroup&lt;TKey, TElement> and ReadOnlyObservableGroup&lt;TKey, TElement>

The [`ObservableGroup<TKey, TElement>`](/dotnet/api/communitytoolkit.mvvm.collections.ObservableGroup-2) and [`ReadOnlyObservableGroup<TKey, TElement>`](/dotnet/api/communitytoolkit.mvvm.collections.ReadOnlyObservableGroup-2) types are custom observable collection types inheriting from [`ObservableCollection<T>`](/dotnet/api/system.collections.objectmodel.ObservableCollection-1) and [`ReadOnlyObservableCollection<T>`](/dotnet/api/system.collections.objectmodel.ReadOnlyObservableCollection-1) that also provide grouping support. This is particularly useful when binding a grouped collection of items to the UI, such as to display a list of contacts.

> **Platform APIs:** [`ObservableGroup<TKey, TElement>`](/dotnet/api/communitytoolkit.mvvm.collections.ObservableGroup-2), [`ReadOnlyObservableGroup<TKey, TElement>`](/dotnet/api/communitytoolkit.mvvm.collections.ReadOnlyObservableGroup-2), [`IReadOnlyObservableGroup`](/dotnet/api/communitytoolkit.mvvm.collections.IReadOnlyObservableGroup), [`IReadOnlyObservableGroup<TKey>`](/dotnet/api/communitytoolkit.mvvm.collections.IReadOnlyObservableGroup-1), [`IReadOnlyObservableGroup<TKey, TElement>`](/dotnet/api/communitytoolkit.mvvm.collections.IReadOnlyObservableGroup-2)

## ObservableGroup&lt;TKey, TElement> features

`ObservableGroup<TKey, TElement>` and `ReadOnlyObservableGroup<TKey, TElement>` have the following main features:

- They inherit from `ObservableCollection<T>` and `ReadOnlyObservableCollection<T>`, thus providing the same notification support for when items are added, removed or modified in the collection.
- They also implement the [`IGrouping<TKey, TElement>`](/dotnet/api/system.linq.IGrouping-2) interface, allowing instances to be used as arguments for all existing LINQ extensions working on instances of this interface.
- They implement several interfaces from the MVVM Toolkit (`IReadOnlyObservableGroup`, `IReadOnlyObservableGroup<TKey>` and `IReadOnlyObservableGroup<TKey, TElement>`) that enable different level of abstraction over instances of these two collection types. This can be especially useful in data templates, where only partial type information is available or can be used.

# ObservableGroupedCollection&lt;TKey, TElement> and ReadOnlyObservableGroupedCollection&lt;TKey, TElement>

The [`ObservableGroupedCollection<TKey, TElement>`](/dotnet/api/communitytoolkit.mvvm.collections.ObservableGroupedCollection-2) and [`ReadOnlyObservableGroupedCollection<TKey, TElement>`](/dotnet/api/communitytoolkit.mvvm.collections.ReadOnlyObservableGroup-2) are observable collection types where each item is a grouped collection type (either `ObservableGroup<TKey, TElement>` or `ReadOnlyObservableGroup<TKey, TElement>`), that also implement [`ILookup<TKey, TElement>`](/dotnet/api/System.Linq.ILookup-2).

> **Platform APIs:** [`ObservableGroupedCollection<TKey, TElement>`](/dotnet/api/communitytoolkit.mvvm.collections.ObservableGroupedCollection-2), [`ReadOnlyObservableGroupedCollection<TKey, TElement>`](/dotnet/api/communitytoolkit.mvvm.collections.ReadOnlyObservableGroup-2), [`ObservableGroupedCollectionExtensions`](/dotnet/api/communitytoolkit.mvvm.collections.ObservableGroupedCollectionExtensions).

## ObservableGroupedCollection&lt;TKey, TElement> features

`ObservableGroupedCollection<TKey, TElement>` and `ReadOnlyObservableGroupedCollection<TKey, TElement>` have the following main features:

- They inherit from `ObservableCollection<T>` and `ReadOnlyObservableCollection<T>`, so just like `ObservableGroup<TKey, TElement>` and `ReadOnlyObservableGroup<TKey, TElement>` they also provide notification when items (groups, in this case) are added, removed or modified. 
- They implement the [`ILookup<TKey, TElement>`](/dotnet/api/system.linq.IGrouping-2) interface, to improve LINQ interoperability.
- They provide additional helper methods to easily interact with groups and items in these collection through all the APIs in the `ObservableGroupedCollectionExtensions` type.

## Working with `ObservableGroupedCollection<TKey, TElement>`

Suppose we had a simple definition of a `Contact` model like this:

```csharp
public sealed record Contact(Name Name, string Email, Picture Picture);

public sealed record Name(string First, string Last)
{
    public override string ToString() => $"{First} {Last}";
}

public sealed record Picture(string Url);
```

And then a viewmodel using `ObservableGroupedCollection<TKey, TElement>` to setup a list of contacts:

```csharp
public class ContactsViewModel : ObservableObject
{
    /// <summary>
    /// The <see cref="IContactsService"/> instance currently in use.
    /// </summary>
    private readonly IContactsService contactsService;

    public ContactsViewModel(IContactsService contactsService) 
    {
        this.contactsService = contactsService;

        LoadContactsCommand = new AsyncRelayCommand(LoadContactsAsync);
    }

    public IAsyncRelayCommand LoadContactsCommand { get; }

    /// <summary>
    /// Gets the current collection of contacts
    /// </summary>
    public ObservableGroupedCollection<string, Contact> Contacts { get; private set; } = new();

    /// <summary>
    /// Loads the contacts to display.
    /// </summary>
    private async Task LoadContactsAsync()
    {
        var contacts = await contactsService.GetContactsAsync();

        Contacts = new ObservableGroupedCollection<string, Contact>(
            contacts.Contacts
            .GroupBy(static c => char.ToUpperInvariant(c.Name.First[0]).ToString())
            .OrderBy(static g => g.Key));

        OnPropertyChanged(nameof(Contacts));
    }
}
```

And the relative UI could then be (using WinUI XAML):

```xml
<UserControl
    x:Class="MvvmSampleUwp.Views.ContactsView"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:collections="using:CommunityToolkit.Common.Collections"
    xmlns:contacts="using:MvvmSample.Core.Models"
    xmlns:core="using:Microsoft.Xaml.Interactions.Core"
    xmlns:interactivity="using:Microsoft.Xaml.Interactivity"
    xmlns:muxc="using:Microsoft.UI.Xaml.Controls"
    xmlns:system="using:System"
    NavigationCacheMode="Enabled">
    <Page.Resources>

        <!--  SemanticZoom grouped source (this connects the grouped collection to the UI)  -->
        <CollectionViewSource
            x:Name="PeopleViewSource"
            IsSourceGrouped="True"
            Source="{x:Bind ViewModel.Contacts, Mode=OneWay}" />

        <!--  Contact template  -->
        <DataTemplate x:Key="PersonListViewTemplate" x:DataType="contacts:Contact">

            <!-- Some custom template here -->
        </DataTemplate>
    </Page.Resources>

    <!--  A command to load contacts when the control is loaded in the visual tree  -->
    <interactivity:Interaction.Behaviors>
        <core:EventTriggerBehavior EventName="Loaded">
            <core:InvokeCommandAction Command="{x:Bind ViewModel.LoadContactsCommand}" />
        </core:EventTriggerBehavior>
    </interactivity:Interaction.Behaviors>

    <Grid>

        <!--  Loading bar (is displayed when the contacts are loading)  -->
        <muxc:ProgressBar
            HorizontalAlignment="Stretch"
            VerticalAlignment="Top"
            Background="Transparent"
            IsIndeterminate="{x:Bind ViewModel.LoadContactsCommand.IsRunning, Mode=OneWay}" />

        <!--  Contacts view  -->
        <SemanticZoom>
            <SemanticZoom.ZoomedInView>
                <ListView
                    ItemTemplate="{StaticResource PersonListViewTemplate}"
                    ItemsSource="{x:Bind PeopleViewSource.View, Mode=OneWay}"
                    SelectionMode="Single">
                    <ListView.GroupStyle>
                        <GroupStyle HidesIfEmpty="True">

                            <!--  Header template (you can see IReadOnlyObservableGroup being used here)  -->
                            <GroupStyle.HeaderTemplate>
                                <DataTemplate x:DataType="collections:IReadOnlyObservableGroup">
                                    <TextBlock
                                        FontSize="24"
                                        Foreground="{ThemeResource SystemControlHighlightAccentBrush}"
                                        Text="{x:Bind Key}" />
                                </DataTemplate>
                            </GroupStyle.HeaderTemplate>
                        </GroupStyle>
                    </ListView.GroupStyle>
                </ListView>
            </SemanticZoom.ZoomedInView>
            <SemanticZoom.ZoomedOutView>
                <GridView
                    HorizontalAlignment="Stretch"
                    ItemsSource="{x:Bind PeopleViewSource.View.CollectionGroups, Mode=OneWay}"
                    SelectionMode="Single">
                    <GridView.ItemTemplate>

                        <!--  Zoomed out header template (IReadOnlyObservableGroup is used again)  -->
                        <DataTemplate x:DataType="ICollectionViewGroup">
                            <Border Width="80" Height="80">
                                <TextBlock
                                    HorizontalAlignment="Center"
                                    VerticalAlignment="Center"
                                    FontSize="32"
                                    Foreground="{ThemeResource SystemControlHighlightAccentBrush}"
                                    Text="{x:Bind Group.(collections:IReadOnlyObservableGroup.Key)}" />
                            </Border>
                        </DataTemplate>
                    </GridView.ItemTemplate>
                </GridView>
            </SemanticZoom.ZoomedOutView>
        </SemanticZoom>
    </Grid>
</UserControl>

```

This would display a grouped list of contacts, with each group using the initial of its contained contacts as title.

When run, the snippet above results in the following UI, from the MVVM Toolkit Sample App:

![](https://user-images.githubusercontent.com/10199417/161963806-d7fecadf-1603-4e06-91eb-b1ed72aafc41.png)

## Examples

- Check out the [sample app](https://github.com/windows-toolkit/MVVM-Samples) (for multiple UI frameworks) to see the MVVM Toolkit in action.
- You can also find more examples in the [unit tests](https://github.com/windows-toolkit/WindowsCommunityToolkit/blob/rel/7.1.0/UnitTests/UnitTests.Shared/Mvvm).
