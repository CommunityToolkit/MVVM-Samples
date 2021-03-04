---
title: ObservableRecipient
author: Sergio0694
description: A base class for observable objects that also acts as recipients for messages
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, componentmodel, property changed, notification, binding, messenger, messaging, net core, net standard
dev_langs:
  - csharp
---

# ObservableRecipient

The [`ObservableRecipient`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.componentmodel.ObservableRecipient) type is a base class for observable objects that also acts as recipients for messages. This class is an extension of [`ObservableObject`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.componentmodel.ObservableObject) which also provides built-in support to use the [`IMessenger`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.Messaging.IMessenger) type.

> **Platform APIs:** [`ObservableRecipient`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.componentmodel.ObservableRecipient), [`ObservableObject`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.componentmodel.ObservableObject), [`IMessenger`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.Messaging.IMessenger), [`WeakReferenceMessenger`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.Messaging.WeakReferenceMessenger), [`IRecipient<TMessage>`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.Messaging.irecipient-1), [`PropertyChangedMessage<T>`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.Messaging.Messages.PropertyChangedMessage-1)

## How it works

The `ObservableRecipient` type is meant to be used as a base for viewmodels that also use the `IMessenger` features, as it provides built-in support for it. In particular:

- It has both a parameterless constructor and one that takes an `IMessenger` instance, to be used with dependency injection. It also exposes a `Messenger` property that can be used to send and receive messages in the viewmodel. If the parameterless constructor is used, the `WeakReferenceMessenger.Default` instance will be assigned to the `Messenger` property.
- It exposes an `IsActive` property to activate/deactivate the viewmodel. In this context, to "activate" means that a given viewmodel is marked as being in use, such that eg. it will start listening for registered messages, perform other setup operations, etc. There are two related methods, `OnActivated` and `OnDeactivated`, that are invoked when the property changes value. By default, `OnDeactivated` automatically unregisters the current instance from all registered messages. For best results and to avoid memory leaks, it's recommended to use `OnActivated` to register to messages, and to use `OnDeactivated` to do cleanup operations. This pattern allows a viewmodel to be enabled/disabled multiple times, while being safe to collect without the risk of memory leaks every time it's deactivated. By default, `OnActived` will automatically register all the message handlers defined through the `IRecipient<TMessage>` interface.
- It exposes a `Broadcast<T>(T, T, string)` method which sends a `PropertyChangedMessage<T>` message through the `IMessenger` instance available from the `Messenger` property. This can be used to easily broadcast changes in the properties of a viewmodel without having to manually retrieve a `Messenger` instance to use. This method is used by the overload of the various `SetProperty` methods, which have an additional `bool broadcast` property to indicate whether or not to also send a message.

Here's an example of a viewmodel that receives `LoggedInUserRequestMessage` messages when active:

```csharp
public class MyViewModel : ObservableRecipient, IRecipient<LoggedInUserRequestMessage>
{
    public void Receive(LoggedInUserRequestMessage message)
    {
        // Handle the message here
    }
}
```

In the example above, `OnActivated` automatically registers the instance as a recipient for `LoggedInUserRequestMessage` messages, using that method as the action to invoke. Using the `IRecipient<TMessage>` interface is not mandatory, and the registration can also be done manually (even using just an inline lambda expression):

```csharp
public class MyViewModel : ObservableRecipient
{
    protected override void OnActivated()
    {
        // Using a method group...
        Messenger.Register<MyViewModel, LoggedInUserRequestMessage>(this, (r, m) => r.Receive(m));

        // ...or a lambda expression
        Messenger.Register<MyViewModel, LoggedInUserRequestMessage>(this, (r, m) =>
        {
            // Handle the message here
        });
    }

    private void Receive(LoggedInUserRequestMessage message)
    {
        // Handle the message here
    }
}
```

## Examples

You can find more examples in the [unit tests](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/UnitTests/UnitTests.Shared/Mvvm).