---
title: Messenger
author: Sergio0694
description: A type that can be used to exchange messages between different objects
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, service, messenger, messaging, net core, net standard
dev_langs:
  - csharp
---

# Messenger

The [`Messenger`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.Messaging.Messenger) class (with the accompanying [`IMessenger`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.Messaging.IMessenger) interface) can be used to exchange messages between different objects. This can be useful to decouple different modules of an application without having to keep strong references to types being referenced. It is also possible to send messages to specific channels, uniquely identified by a token, and to have different messengers in different sections of an application. 

## How it works

The `Messenger` type is responsible for maintaining links between recipients (receivers of messages) and their registered message types, with relative message handlers. Any object can be registered as a recipient for a given message type using a message handler, which will be invoked whenever the `Messenger` instance is used to send a message of that type. It is also possible to send messages through specific communication channels (each identified by a unique token), so that multiple modules can exchange messages of the same type without causing conflicts. Messages sent without a token use the default shared channel.

There are two ways to perform message registration: either through the `IRecipient<TMessage>` interface, or using an `Action<TMessage>` delegate acting as message handler. The first lets you register all the handlers with a single call to the `RegisterAll` extension, which automatically registers the recipients of all the declared message handlers, while the latter is useful when you need more flexibility or when you want to use a simple lambda expression as a message handler.

Similar to the `Ioc` class, `Messenger` exposes a `Default` property that offers a thread-safe implementation built-in into the package. It is also possible to create multiple `Messenger` instances if needed, for instance if a different one is injected with a DI service provider into a different module of the app (for instance, multiple windows running in the same process).

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
Messenger.Default.Register<LoggedInUserChangedMessage>(this, m =>
{
    // Handle the message here
});

// Send a message from some other module
Messenger.Default.Send(new LoggedInUserChangedMessage(user));
```

The `Messenger` class takes care of delivering messages to all the registered recipients. Note that a recipient can subscribe to messages of a specific type. Note that inherited message types are not registered in the default `Messenger` implementation.

In order to avoid memory leaks, remember to unregister recipients when you don't need them anymore. You can unregister either by message type, by registration token, or by recipient:

```csharp
// Unregisters the recipient from a message type
Messenger.Default.Unregister<LoggedInUserChangedMessage>(this);

// Unregisters the recipient from a message type in a specified channel
Messenger.Default.Unregister<LoggedInUserChangedMessage, int>(this, 42);

// Unregister the recipient from all messages, across all channels
Messenger.Default.UnregisterAll(this);
```

## Using request messages

Another useful feature of messenger instances is that they can also be used to request values from a module to another. In order to do so, the package includes a base `RequestMessage<T>` class, which can be used like so:

```csharp
// Create a message
public class LoggedInUserRequestMessage : RequestMessage<User>
{
}

// Register the receiver in a module
Messenger.Default.Register<LoggedInUserRequestMessage>(this, m =>
{
    m.Reply(CurrentUser); // Assume this is a private member
});

// Request the value from another module
User user = Messenger.Default.Send<LoggedInUserRequestMessage>();
```

The `RequestMessage<T>` class includes an implicit converter that makes the conversion from a `LoggedInUserRequestMessage` to its contained `User` object possible. This will also check that a response has been received for the message, and throw an exception if that's not the case. It is also possible to send request messages without this mandatory response guarantee: just store the returned message in a local variable, and then manually check whether a response value is available or not. Doing so will not trigger the automatic exception if a response is not received when the `Send` method returns.

The same namespace also includes base requests message for other scenarios: `AsyncRequestMessage<T>`, `CollectionRequestMessage<T>` and `AsyncCollectionRequestMessage<T>`.
Here's how you can use an async request message:

```csharp
// Create a message
public class LoggedInUserRequestMessage : AsyncRequestMessage<User>
{
}

// Register the receiver in a module
Messenger.Default.Register<LoggedInUserRequestMessage>(this, m =>
{
    m.Reply(GetCurrentUserAsync()); // We're replying with a Task<User>
});

// Request the value from another module (we can directly await on the request)
User user = await Messenger.Default.Send<LoggedInUserRequestMessage>();
```

## Sample Code

You can find more examples in our [unit tests](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/UnitTests/UnitTests.Shared/Mvvm)

## Requirements

| Device family | Universal, 10.0.16299.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Mvvm |
| NuGet package | [Microsoft.Toolkit.Mvvm](https://www.nuget.org/packages/Microsoft.Toolkit.Mvvm/) |

## API

* [Messenger source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Mvvm/Messaging/Messenger.cs)
* [IMessenger source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Mvvm/Messaging/IMessenger.cs)
