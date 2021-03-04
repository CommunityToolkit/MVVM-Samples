---
title: ObservableObject
author: Sergio0694
description: A base class for objects of which the properties must be observable
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, componentmodel, property changed, notification, binding, net core, net standard
dev_langs:
  - csharp
---

# ObservableObject

The [`ObservableObject`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.componentmodel.ObservableObject) is a base class for objects that are observable by implementing the [`INotifyPropertyChanged`](https://docs.microsoft.com/dotnet/api/system.componentmodel.inotifypropertychanged) and [`INotifyPropertyChanging`](https://docs.microsoft.com/dotnet/api/system.componentmodel.inotifypropertychanging) interfaces. It can be used as a starting point for all kinds of objects that need to support property change notifications.

> **Platform APIs:** [`ObservableObject`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.componentmodel.ObservableObject), [`TaskNotifier`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.componentmodel.ObservableObject.TaskNotifier), [`TaskNotifier<T>`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.componentmodel.ObservableObject.TaskNotifier-1)

## How it works

`ObservableObject` has the following main features:

- It provides a base implementation for `INotifyPropertyChanged` and `INotifyPropertyChanging`, exposing the `PropertyChanged` and `PropertyChanging` events.
- It provides a series of `SetProperty` methods that can be used to easily set property values from types inheriting from `ObservableObject`, and to automatically raise the appropriate events.
- It provides the `SetPropertyAndNotifyOnCompletion` method, which is analogous to `SetProperty` but with the ability to set `Task` properties and raise the notification events automatically when the assigned tasks are completed.
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
        set => SetProperty(ref name, value);
    }
}
```

The provided `SetProperty<T>(ref T, T, string)` method checks the current value of the property, and updates it if different, and then also raises the relevant events automatically. The property name is automatically captured through the use of the `[CallerMemberName]` attribute, so there's no need to manually specify which property is being updated.

## Wrapping a non-observable model

A common scenario, for instance, when working with database items, is to create a wrapping "bindable" model that relays properties of the database model, and raises the property changed notifications when needed. This is also needed when wanting to inject notification support to models, that don't implement the `INotifyPropertyChanged` interface. `ObservableObject` provides a dedicated method to make this process simpler. For the following example, `User` is a model directly mapping a database table, without inheriting from `ObservableObject`:

```csharp
public class ObservableUser : ObservableObject
{
    private readonly User user;

    public ObservableUser(User user) => this.user = user;

    public string Name
    {
        get => user.Name;
        set => SetProperty(user.Name, value, user, (u, n) => u.Name = n);
    }
}
```

In this case we're using the `SetProperty<TModel, T>(T, T, TModel, Action<TModel, T>, string)` overload. The signature is slightly more complex than the previous one - this is necessary to let the code still be extremely efficient even if we don't have access to a backing field like in the previous scenario. We can go through each part of this method signature in detail to understand the role of the different components:

- `TModel` is a type argument, indicating the type of the model we're wrapping. In this case, it'll be our `User` class. Note that we don't need to specify this explicitly - the C# compiler will infer this automatically by how we're invoking the `SetProperty` method.
- `T` is the type of the property we want to set. Similarly to `TModel`, this is inferred automatically.
- `T oldValue` is the first parameter, and in this case we're using `user.Name` to pass the current value of that property we're wrapping.
- `T newValue` is the new value to set to the property, and here we're passing `value`, which is the input value within the property setter.
- `TModel model` is the target model we are wrapping, in this case we're passing the instance stored in the `user` field.
- `Action<TModel, T> callback` is a function that will be invoked if the new value of the property is different than the current one, and the property needs to be set. This will be done by this callback function, which receives as input the target model and the new property value to set. In this case we're just assigning the input value (which we called `n`) to the `Name` property (by doing `u.Name = n`). It is important here to avoid capturing values from the current scope and only interact with the ones given as input to the callback, as this allows the C# compiler to cache the callback function and perform a number of performance improvements. It's because of this that we're not just directly accessing the `user` field here or the `value` parameter in the setter, but instead we're only using the input parameters for the lambda expression.

The `SetProperty<TModel, T>(T, T, TModel, Action<TModel, T>, string)` method makes creating these wrapping properties extremely simple, as it takes care of both retrieving and setting the target properties while providing an extremely compact API.

> [!NOTE]
> Compared to the implementation of this method using LINQ expressions, specifically through a parameter of type `Expression<Func<T>>` instead of the state and callback parameters, the performance improvements that can be achieved this way are really significant. In particular, this version is ~200x faster than the one using LINQ expressions, and does not make any memory allocations at all.

## Handling `Task<T>` properties

If a property is a `Task` it's necessary to also raise the notification event once the task completes, so that bindings are updated at the right time. eg. to display a loading indicator or other status info on the operation represented by the task. `ObservableObject` has an API for this scenario:

```csharp
public class MyModel : ObservableObject
{
    private TaskNotifier<int>? requestTask;

    public Task<int>? RequestTask
    {
        get => requestTask;
        set => SetPropertyAndNotifyOnCompletion(ref requestTask, value);
    }

    public void RequestValue()
    {
        RequestTask = WebService.LoadMyValueAsync();
    }
}
```

Here the `SetPropertyAndNotifyOnCompletion<T>(ref TaskNotifier<T>, Task<T>, string)` method will take care of updating the target field, monitoring the new task, if present, and raising the notification event when that task completes. This way, it's possible to just bind to a task property and to be notified when its status changes. The [`TaskNotifier<T>`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.componentmodel.ObservableObject.TaskNotifier-1) is a special type exposed by `ObservableObject` that wraps a target `Task<T>` instance and enables the necessary notification logic for this method. The [`TaskNotifier`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.componentmodel.ObservableObject.TaskNotifier) type is also available to use directly if you have a general `Task` only.

> [!NOTE]
> The `SetPropertyAndNotifyOnCompletion` method is meant to replace the usage of the `NotifyTaskCompletion<T>` type from the `Microsoft.Toolkit` package. If this type was being used, it can be replaced with just the inner `Task` (or `Task<TResult>`) property, and then the `SetPropertyAndNotifyOnCompletion` method can be used to set its value and raise notification changes. All the properties exposed by the `NotifyTaskCompletion<T>` type are available directly on `Task` instances.

## Examples

You can find more examples in the [unit tests](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/UnitTests/UnitTests.Shared/Mvvm).