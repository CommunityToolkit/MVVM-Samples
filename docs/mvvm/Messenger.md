---
title: Messenger
author: Sergio0694
description: A type that can be used to exchange messages between different objects
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, service, messenger, messaging, net core, net standard
dev_langs:
  - csharp
---

# Messenger

The [`IMessenger`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.IMessenger) interface is a contract for types that can be used to exchange messages between different objects. This can be useful to decouple different modules of an application without having to keep strong references to types being referenced. It is also possible to send messages to specific channels, uniquely identified by a token, and to have different messengers in different sections of an application. The MVVM Toolkit provides two implementations out of the box: [`WeakReferenceMessenger`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.WeakReferenceMessenger) and [`StrongReferenceMessenger`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.StrongReferenceMessenger): the former uses weak references internally, offering automatic memory management for recipients, while the latter uses strong references and requires developers to manually unsubscribe their recipients when they're no longer needed (more details about how to unregister message handlers can be found below), but in exchange for that offers better performance and far less memory usage.

> **Platform APIs:** [`IMessenger`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.IMessenger), [`WeakReferenceMessenger`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.WeakReferenceMessenger), [`StrongReferenceMessenger`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.StrongReferenceMessenger), [`IRecipient<TMessage>`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.irecipient-1), [`MessageHandler<TRecipient, TMessage>`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.messagehandler-2), [`ObservableRecipient`](/dotnet/api/microsoft.toolkit.mvvm.ComponentModel.ObservableRecipient), [`RequestMessage<T>`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.Messages.RequestMessage-1), [`AsyncRequestMessage<T>`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.Messages.AsyncRequestMessage-1), [`CollectionRequestMessage<T>`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.Messages.CollectionRequestMessage-1), [`AsyncCollectionRequestMessage<T>`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.Messages.AsyncCollectionRequestMessage-1).

## How it works

Types implementing `IMessenger` are responsible for maintaining links between recipients (receivers of messages) and their registered message types, with relative message handlers. Any object can be registered as a recipient for a given message type using a message handler, which will be invoked whenever the `IMessenger` instance is used to send a message of that type. It is also possible to send messages through specific communication channels (each identified by a unique token), so that multiple modules can exchange messages of the same type without causing conflicts. Messages sent without a token use the default shared channel.

There are two ways to perform message registration: either through the [`IRecipient<TMessage>`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.irecipient-1) interface, or using a [`MessageHandler<TRecipient, TMessage>`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.messagehandler-2) delegate acting as message handler. The first lets you register all the handlers with a single call to the `RegisterAll` extension, which automatically registers the recipients of all the declared message handlers, while the latter is useful when you need more flexibility or when you want to use a simple lambda expression as a message handler.

Both `WeakReferenceMessenger` and `StrongReferenceMessenger` also expose a `Default` property that offers a thread-safe implementation built-in into the package. It is also possible to create multiple messenger instances if needed, for instance if a different one is injected with a DI service provider into a different module of the app (for instance, multiple windows running in the same process).

> [!NOTE]
> Since the `WeakReferenceMessenger` type is simpler to use and matches the behavior of the messenger type from the `MvvmLight` library, it is the default type being used by the [`ObservableRecipient`](ObservableRecipient.md) type in the MVVM Toolkit. The `StrongReferenceType` can still be used, by passing an instance to the constructor of that class.

## Sending and receiving messages

Consider the following:

```csharp
// Create a message
public class LoggedInUserChangedMessage : ValueChangedMessage<User>
{
    public LoggedInUserChangedMessage(User user) : base(user)
    {        
    }
}

// Register a message in some module
WeakReferenceMessenger.Default.Register<LoggedInUserChangedMessage>(this, (r, m) =>
{
    // Handle the message here, with r being the recipient and m being the
    // input message. Using the recipient passed as input makes it so that
    // the lambda expression doesn't capture "this", improving performance.
});

// Send a message from some other module
WeakReferenceMessenger.Default.Send(new LoggedInUserChangedMessage(user));
```

Let's imagine this message type being used in a simple messaging application, which displays a header with the user name and profile image of the currently logged user, a panel with a list of conversations, and another panel with messages from the current conversation, if one is selected. Let's say these three sections are supported by the `HeaderViewModel`, `ConversationsListViewModel` and `ConversationViewModel` types respectively. In this scenario, the `LoggedInUserChangedMessage` message might be sent by the `HeaderViewModel` after a login operation has completed, and both those other viewmodels might register handlers for it. For instance, `ConversationsListViewModel` will load the list of conversations for the new user, and `ConversationViewModel` will just close the current conversation, if one is present.

The `IMessenger` instance takes care of delivering messages to all the registered recipients. Note that a recipient can subscribe to messages of a specific type. Note that inherited message types are not registered in the default `IMessenger` implementations provided by the MVVM Toolkit.

When a recipient is not needed anymore, you should unregister it so that it will stop receiving messages. You can unregister either by message type, by registration token, or by recipient:

```csharp
// Unregisters the recipient from a message type
WeakReferenceMessenger.Default.Unregister<LoggedInUserChangedMessage>(this);

// Unregisters the recipient from a message type in a specified channel
WeakReferenceMessenger.Default.Unregister<LoggedInUserChangedMessage, int>(this, 42);

// Unregister the recipient from all messages, across all channels
WeakReferenceMessenger.Default.UnregisterAll(this);
```

> [!WARNING]
> As mentioned before, this is not strictly necessary when using the `WeakReferenceMessenger` type, as it uses weak references to track recipients, meaning that unused recipients will still be eligible for garbage collection even though they still have active message handlers. It is still good practice to unsubscribe them though, to improve performances. On the other hand, the `StrongReferenceMessenger` implementation uses strong references to track the registered recipients. This is done for performance reasons, and it means that each registered recipient should manually be unregistered to avoid memory leaks. That is, as long as a recipient is registered, the `StrongReferenceMessenger` instance in use will keep an active reference to it, which will prevent the garbage collector from being able to collect that instance. You can either handle this manually, or you can inherit from `ObservableRecipient`, which by default automatically takes care of removing all the message registrations for recipient when it is deactivated (see docs on `ObservableRecipient` for more info about this).

It is also possible to use the `IRecipient<TMessage>` interface to register message handlers. In this case, each recipient will need to implement the interface for a given message type, and provide a `Receive(TMessage)` method that will be invoke when receiving messages, like so:

```csharp
// Create a message
public class MyRecipient : IRecipient<LoggedInUserChangedMessage>
{
    public void Receive(LoggedInUserChangedMessage message)
    {
        // Handle the message here...   
    }
}

// Register that specific message...
WeakReferenceMessenger.Default.Register<LoggedInUserChangedMessage>(this);

// ...or alternatively, register all declared handlers
WeakReferenceMessenger.Default.RegisterAll(this);

// Send a message from some other module
WeakReferenceMessenger.Default.Send(new LoggedInUserChangedMessage(user));
```

## Using request messages

Another useful feature of messenger instances is that they can also be used to request values from a module to another. In order to do so, the package includes a base [`RequestMessage<T>`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.Messages.RequestMessage-1) class, which can be used like so:

```csharp
// Create a message
public class LoggedInUserRequestMessage : RequestMessage<User>
{
}

// Register the receiver in a module
WeakReferenceMessenger.Default.Register<MyViewModel, LoggedInUserRequestMessage>(this, (r, m) =>
{
    // Assume that "CurrentUser" is a private member in our viewmodel.
    // As before, we're accessing it through the recipient passed as
    // input to the handler, to avoid capturing "this" in the delegate.
    m.Reply(r.CurrentUser);
});

// Request the value from another module
User user = WeakReferenceMessenger.Default.Send<LoggedInUserRequestMessage>();
```

The `RequestMessage<T>` class includes an implicit converter that makes the conversion from a `LoggedInUserRequestMessage` to its contained `User` object possible. This will also check that a response has been received for the message, and throw an exception if that's not the case. It is also possible to send request messages without this mandatory response guarantee: just store the returned message in a local variable, and then manually check whether a response value is available or not. Doing so will not trigger the automatic exception if a response is not received when the `Send` method returns.

The same namespace also includes base requests message for other scenarios: [`AsyncRequestMessage<T>`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.Messages.AsyncRequestMessage-1), [`CollectionRequestMessage<T>`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.Messages.CollectionRequestMessage-1) and [`AsyncCollectionRequestMessage<T>`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.Messages.AsyncCollectionRequestMessage-1).
Here's how you can use an async request message:

```csharp
// Create a message
public class LoggedInUserRequestMessage : AsyncRequestMessage<User>
{
}

// Register the receiver in a module
WeakReferenceMessenger.Default.Register<MyViewModel, LoggedInUserRequestMessage>(this, (r, m) =>
{
    m.Reply(r.GetCurrentUserAsync()); // We're replying with a Task<User>
});

// Request the value from another module (we can directly await on the request)
User user = await WeakReferenceMessenger.Default.Send<LoggedInUserRequestMessage>();
```

## Examples

- Check out the [sample app](https://github.com/windows-toolkit/MVVM-Samples) (for multiple UI frameworks) to see the MVVM Toolkit in action.
- You can also find more examples in the [unit tests](https://github.com/windows-toolkit/WindowsCommunityToolkit/blob/rel/7.1.0/UnitTests/UnitTests.Shared/Mvvm).
