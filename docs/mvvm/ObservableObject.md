---
title: ObservableObject
author: Sergio0694
description: A base class for objects of which the properties must be observable
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, componentmodel, property changed, notification, binding, net core, net standard
dev_langs:
  - csharp
---

# ObservableObject

The [ObservableObject](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.componentmodel.ObservableObject) is a base class for objects of which the properties must be observable, implementing the [`INotifyPropertyChanged`](https://docs.microsoft.com/dotnet/api/system.componentmodel.inotifypropertychanged) and [`INotifyPropertyChanging`](https://docs.microsoft.com/dotnet/api/system.componentmodel.inotifypropertychanging) interfaces. It can be used as a starting point for all kinds of objects that need to support property change notifications.

## How it works

`ObservableObject` has the following main features:

- It provides a base implementation for `INotifyPropertyChanged` and `INotifyPropertyChanging`, exposing the `PropertyChanged` and `PropertyChanging` events.
- It provides a series of `Set` methods that can be used to easily set property values from types inheriting from `ObservableObject`, and to automatically raise the appropriate events.
- It provides the `SetAndNotifyOnCompletion` method, which is analogous to `Set` but with the ability to set `Task` properties and raise the notification events automatically when the assigned tasks are completed.
- It exposes the `OnPropertyChanged` and `OnPropertyChanging` methods, which can be overridden in derived types to customize how the notification events are raised.

## Simple property

Here's an example of how to implement notification support to a custom property:

```csharp
public class User : ObservableObject
{
    private string name;

    public string Name
    {
        get => name;
        set => Set(ref name, value);
    }
}
```

The provided `Set<T>(ref T, T, string)` method can be used to check the current value of the property, and update it if needed, and then also raise the relevant events automatically. The property name is automatically captured through the use of the `[CallerMemberName]` attribute, so there's no need to manually specify which property is being updated.

## Wrapping a non-observable model

A common scenario, for instance when working with database items, is to create a wrapping "bindable" model that simply relays property to the database model, and raises the property changed notifications when needed. This is also needed when we want to inject notification support to models we don't own, that don't directly implement the `INotifyPropertyChanged` interface. `ObservableObject` provides a dedicated method to make this process simpler. For the following example, imagine that `User` was a model directly mapping a database table, without inheriting from `ObservableObject`:

```csharp
public class ObservableUser : ObservableObject
{
    private readonly User user;

    public ObservableUser(User user) => this.user = user;

    public string Name
    {
        get => user.Name;
        set => Set(() => user.Name, value);
    }
}
```

The `Set<T>(Expression<Func<T>>, T, string)` method makes creating these wrapping properties extremely simple, as it takes of both retrieving and setting the target properties while providing an extremely compact API to do so.

## Handling `Task<T>` properties

Lastly, what if the property we want to set is a `Task`? In that case, we'd wait to also raise the notification event when the task completes, so that we could bind to that property eg. to display some loading indicator or some other status info on the operation represented by the task. `ObservableObject` has a useful API for this scenario as well:

```csharp
public class MyModel : ObservableObject
{
    private Task<int> requestTask;

    public Task<int> RequestTask
    {
        get => requestTask;
        set => SetAndNotifyOnCompletion(ref requestTask, () => requestTask, value);
    }

    public void RequestValue()
    {
        LoadingTask = WebService.LoadMyValueAsync();
    }
}
```

Here the `SetAndNotifyOnCompletion<TTask>(ref TTask, Expression<Func<TTask>>, TTask, string)` method will take care of updating the target field, then monitoring the new task, if present, and raising the notification event again when that task completes. This way, it's possible to just bind to that task property and to be notified when its status changes.

## Sample Code

You can find more examples in our [unit tests](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/UnitTests/UnitTests.Shared/Mvvm)

## Requirements

| Device family | Universal, 10.0.16299.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.HighPerformance |
| NuGet package | [Microsoft.Toolkit.HighPerformance](https://www.nuget.org/packages/Microsoft.Toolkit.HighPerformance/) |

## API

* [ObservableObject source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Mvvm/ComponentModel/ObservableObject.cs)
