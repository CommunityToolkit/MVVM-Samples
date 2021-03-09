---
title: Migrating from MvvmLight
author: jamesmcroft
description: This article describes how to migrate MvvmLight solutions to the Windows Community Toolkit MVVM Toolkit.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, mvvmlight, net core, net standard
dev_langs:
  - csharp
---

# Migrating from MvvmLight

This article outlines some of the key differences between the [MvvmLight Toolkit](https://github.com/lbugnion/mvvmlight) and the MVVM Toolkit to ease your migration. 

While this article specifically focuses on the migrations from MvvmLight to the MVVM Toolkit, note that there are additional improvements that have been made within the MVVM Toolkit, so it is highly recommend taking a look at the documentation for the individual new APIs.

> **Platform APIs:** [`ObservableObject`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.componentmodel.ObservableObject), [`ObservableRecipient`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.componentmodel.ObservableRecipient), [`RelayCommand`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.input.RelayCommand), [`RelayCommand<T>`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.input.RelayCommand-1), [`AsyncRelayCommand`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.input.AsyncRelayCommand), [`AsyncRelayCommand<T>`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.input.AsyncRelayCommand-1), [`IMessenger`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.Messaging.IMessenger), [`WeakReferenceMessenger`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.Messaging.WeakReferenceMessenger), [`StrongReferenceMessenger`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.Messaging.StrongReferenceMessenger), [`IRecipient<TMessage>`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.Messaging.irecipient-1), [`MessageHandler<TRecipient, TMessage>`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.Messaging.messagehandler-2), [`IMessengerExtensions`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.Messaging.IMessengerExtensions)

## Installing the WCT MVVM Toolkit

To take advantage of the Windows Community Toolkit MVVM framework, you'll first need to install the latest NuGet package to your existing Windows application.

### Install via .NET CLI

```
dotnet add package Microsoft.Toolkit.Mvvm --version 7.0.0
```

### Install via PackageReference

```xml
<PackageReference Include="Microsoft.Toolkit.Mvvm" Version="7.0.0" />
```

## Migrating ObservableObject

The following steps focus on migrating your existing components which take advantage of the `ObservableObject` of the MvvmLight Toolkit. The Windows Community Toolkit MVVM framework provides an [`ObservableObject`](ObservableObject.md) type that is similar. 

The first change here will be swapping using directives in your components.

```csharp
// MvvmLight
using GalaSoft.MvvmLight;

// Toolkit.Mvvm
using Microsoft.Toolkit.Mvvm.ComponentModel;
```

Below are a list of migrations that will need to be performed if being used in your current solution.

### ObservableObject methods

#### Set<T>(Expression, ref T, T)

`Set(Expression, ref T, T)` does not have a like-for-like method signature replacement.

However, `SetProperty(ref T, T, string)` provides the same functionality with additional performance benefits. 

```csharp
// MvvmLight
Set(() => MyProperty, ref this.myProperty, value);

// Toolkit.Mvvm
SetProperty(ref this.myProperty, value);
```

Note that the `string` parameter is not required if the method is being called from the property's setter as it is inferred from the caller member name, as can be seen here. If you want to invoke `SetProperty` for a property that is different from the one where the method is being invoked, you can do so by using the `nameof` operator, which can be useful to make the code less error prone by not having hardcoded names. For instance:

```csharp
SetProperty(ref this.someProperty, value, nameof(SomeProperty));
```

#### Set<T>(string, ref T, T)

`Set<T>(string, ref T, T)` does not have a like-for-like method signature replacement.

However, `SetProperty<T>(ref T, T, string)` provides the same functionality with re-ordered parameters.

```csharp
// MvvmLight
Set(nameof(MyProperty), ref this.myProperty, value);

// Toolkit.Mvvm
SetProperty(ref this.myProperty, value);
```

#### Set<T>(ref T, T, string)

`Set<T>(ref T, T, string)` has a renamed direct replacement, `SetProperty<T>(ref T, T, string)`.

```csharp
// MvvmLight
Set(ref this.myProperty, value, nameof(MyProperty));

// Toolkit.Mvvm
SetProperty(ref this.myProperty, value);
```

#### RaisePropertyChanged(string)

`RaisePropertyChanged(string)` has a renamed direct replacement, `OnPropertyChanged(string)`.

```csharp
// MvvmLight
RaisePropertyChanged(nameof(MyProperty));

// Toolkit.Mvvm
OnPropertyChanged();
```

As with `SetProperty`, the name of the current property is automatically inferred by the `OnPropertyChanged` method. If you want to use this method to manually raise the `PropertyChanged` event for another property, you can also manually specify the name of that property by using the `nameof` operator again. For instance:

```csharp
OnPropertyChanged(nameof(SomeProperty));
```

#### RaisePropertyChanged<T>(Expression)

`RaisePropertyChanged<T>(Expression)` does not have a direct replacement.

It is recommended for improved performance that you replace `RaisePropertyChanged<T>(Expression)` with the Toolkit's `OnPropertyChanged(string)` using the `nameof` keyword instead (or with no parameters, if the target property is the same as the one calling the method, so the name can be inferred automatically as mentioned above).

```csharp
// MvvmLight
RaisePropertyChanged(() => MyProperty);

// Toolkit.Mvvm
OnPropertyChanged(nameof(MyProperty));
```

#### VerifyPropertyName(string)

There is no direct replacement for the `VerifyPropertyName(string)` method and any code using this should be altered or removed.

The reason for the omission from the MVVM Toolkit is that using the `nameof` keyword for a property verifies that it exists. When MvvmLight was built, the `nameof` keyword was not available and this method was used to ensure that the property existed on the object.

```csharp
// MvvmLight
VerifyPropertyName(nameof(MyProperty));

// Toolkit.Mvvm
// No direct replacement, remove
```

### ObservableObject properties

#### PropertyChangedHandler

`PropertyChangedHandler` does not have a direct replacement.

To raise a property changed event via the `PropertyChanged` event handler, you need to call the `OnPropertyChanged` method instead. 

```csharp
// MvvmLight
PropertyChangedEventHandler handler = PropertyChangedHandler;

// Toolkit.Mvvm
OnPropertyChanged();
```

## Migrating ViewModelBase

The following steps focus on migrating your existing components which take advantage of the `ViewModelBase` of the MvvmLight Toolkit. 

The Windows Community Toolkit MVVM framework provides an [`ObservableRecipient`](ObservableRecipient.md) type that provides similar functionality. 

Below are a list of migrations that will need to be performed if being used in your current solution.

### ViewModelBase methods

#### Set<T>(string, ref T, T, bool)

`Set<T>(string, ref T, T, bool)` does not have a like-for-like method signature replacement. 

However, `SetProperty<T>(ref T, T, bool, string)` provides the same functionality with re-ordered parameters.

```csharp
// MvvmLight
Set(nameof(MyProperty), ref this.myProperty, value, true);

// Toolkit.Mvvm
SetProperty(ref this.myProperty, value, true);
```

Note, the value and broadcast boolean parameters are not optional in the Toolkit's implementation and must be provided to use this method. The reason for this change is that by omitting the broadcast parameter when calling this method, it will by default call the ObservableObject's `SetProperty` method.

Also, the `string` parameter is not required if the method is being called from the property's setter as it is inferred from the caller member name, just like with the methods in the base `ObservableObject` class.

#### Set<T>(ref T, T, bool, string)

`Set<T>(ref T, T, bool, string)` has a renamed direct replacement, `SetProperty<T>(ref T, T, bool, string)`. 

```csharp
// MvvmLight
Set(ref this.myProperty, value, true, nameof(MyProperty));

// Toolkit.Mvvm
SetProperty(ref this.myProperty, value, true);
```

#### Set<T>(Expression, ref T, T, bool)

`Set<T>(Expression, ref T, T, bool)` does not have a direct replacement. 

It is recommended for improved performance that you replace this with the Toolkit's `SetProperty<T>(ref T, T, bool, string)` using the `nameof` keyword instead.

```csharp
// MvvmLight
Set<MyObject>(() => MyProperty, ref this.myProperty, value, true);

// Toolkit.Mvvm
SetProperty(ref this.myProperty, value, true);
```

#### Broadcast<T>(T, T, string)

`Broadcast<T>(T, T, string)` has a direct replacement which doesn't require a rename. 

```csharp
// MvvmLight
Broadcast<MyObject>(oldValue, newValue, nameof(MyProperty));

// Toolkit.Mvvm
Broadcast(oldValue, newValue, nameof(MyProperty));
```

Note, the message sent via the `Messenger` property when calling the `Broadcast` method has a direct replacement for `PropertyChangedMessage` within the Toolkit's MVVM library.

#### RaisePropertyChanged<T>(string, T, T, bool)

There is no direct replacement for the `RaisePropertyChanged<T>(string, T, T, bool)` method.

The simplest alternative is to call `OnPropertyChanged` and subsequently call `Broadcast` to achieve this functionality.

```csharp
// MvvmLight
RaisePropertyChanged<MyObject>(nameof(MyProperty), oldValue, newValue, true);

// Toolkit.Mvvm
OnPropertyChanged();
Broadcast(oldValue, newValue, nameof(MyProperty));
```

#### RaisePropertyChanged<T>(Expression, T, T, bool)

There is no direct replacement for the `RaisePropertyChanged<T>(Expression, T, T, bool)` method.

The simplest alternative is to call `OnPropertyChanged` and subsequently call `Broadcast` to achieve this functionality.

```csharp
// MvvmLight
RaisePropertyChanged<MyObject>(() => MyProperty, oldValue, newValue, true);

// Toolkit.Mvvm
OnPropertyChanged(nameof(MyProperty));
Broadcast(oldValue, newValue, nameof(MyProperty));
```

#### ICleanup.Cleanup()

There is no direct replacement for the `ICleanup` interface.

However, the `ObservableRecipient` provides an `OnDeactivated` method which should be used to provide the same functionality as `Cleanup`.

`OnDeactivated` in the MVVM Toolkit will also unregister all of the registered messenger events when called.

```csharp
// MvvmLight
Cleanup();

// Toolkit.Mvvm
OnDeactivated();
```

Note, the `OnActivated` and `OnDeactivated` methods can be called from your existing solution as with `Cleanup`. 

However, the `ObservableRecipient` exposes an `IsActive` property that also controls the call to these methods when it is set. 

### ViewModelBase properties

#### MessengerInstance

`MessengerInstance` has a renamed direct replacement, `Messenger`.

```csharp
// MvvmLight
IMessenger messenger = MessengerInstance;

// Toolkit.Mvvm
IMessenger messenger = Messenger;
```

> [!NOTE]
> The default value of the `Messenger` property will be the `WeakReferenceMessenger.Default` instance, which is the standard weak reference messenger implementation in the MVVM Toolkit. This can be customized by just injecting a different `IMessenger` instance into the `ObservableRecipient` constructor.

#### IsInDesignMode

There is no direct replacement for the `IsInDesignMode` property and any code using this should be altered or removed.

The reason for the omission from the MVVM Toolkit is that the `IsInDesignMode` property exposed platform-specific implementations. The MVVM Toolkit has been designed to be platform agnostic.

```csharp
// MvvmLight
var isInDesignMode = IsInDesignMode;

// Toolkit.Mvvm
// No direct replacement, remove
```

### ViewModelBase static properties

#### IsInDesignModeStatic

There is no direct replacement for the `IsInDesignModeStatic` property and any code using this should be altered or removed.

The reason for the omission from the MVVM Toolkit is that the `IsInDesignMode` property exposed platform-specific implementations. The MVVM Toolkit has been designed to be platform agnostic.

```csharp
// MvvmLight
var isInDesignMode = ViewModelBase.IsInDesignModeStatic;

// Toolkit.Mvvm
// No direct replacement, remove
```

## Migrating RelayCommand

The following steps focus on migrating your existing components which take advantage of the `RelayCommand` of the MvvmLight Toolkit. 

The Windows Community Toolkit MVVM framework provides a [`RelayCommand`](RelayCommand.md) type that provides like-for-like functionality taking advantage of the `ICommand` System interface. 

Below are a list of migrations that will need to be performed if being used in your current solution. Where a method or property isn't listed, there is a direct replacement with the same name in the MVVM Toolkit and there is no change required.

The first change here will be swapping using directives in your components.

```csharp
// MvvmLight
using GalaSoft.MvvmLight.Command;
using Galasoft.MvvmLight.CommandWpf;

// Toolkit.Mvvm
using Microsoft.Toolkit.Mvvm.Input;
```

> [!NOTE]
> MvvmLight uses weak references to establish the link between the command and the action called from the associated class. This is not required by the MVVM Toolkit implementation and if this optional parameter has been set to `true` in any of your constructors, this will be removed.

### Using RelayCommand with asynchronous actions

If you are currently using the MvvmLight `RelayCommand` implementation with asynchronous actions, the MVVM Toolkit exposes an improved implementation for these scenarios. 

You can simply replace your existing `RelayCommand` with the `AsyncRelayCommand` which has been built for asynchronous purposes.

```csharp
// MvvmLight
var command = new RelayCommand(() => OnCommandAsync());
var command = new RelayCommand(async () => await OnCommandAsync());

// Toolkit.Mvvm
var asyncCommand = new AsyncRelayCommand(OnCommandAsync);
```

### RelayCommand methods

#### RaiseCanExecuteChanged()

The functionality of `RaiseCanExecuteChanged()` can be achieved with the MVVM Toolkit's `NotifyCanExecuteChanged()` method. 

```csharp
// MvvmLight
var command = new RelayCommand(OnCommand);
command.RaiseCanExecuteChanged();

// Toolkit.Mvvm
var command = new RelayCommand(OnCommand);
command.NotifyCanExecuteChanged();
```

## Migrating RelayCommand<T>

The following steps focus on migrating your existing components which take advantage of the `RelayCommand<T>` of the MvvmLight Toolkit. 

The Windows Community Toolkit MVVM framework provides a [`RelayCommand<T>`](RelayCommand/md) type that provides like-for-like functionality taking advantage of the `ICommand` System interface. 

Below are a list of migrations that will need to be performed if being used in your current solution. Where a method or property isn't listed, there is a direct replacement with the same name in the MVVM Toolkit and there is no change required.

The first change here will be swapping using directives in your components.

```csharp
// MvvmLight
using GalaSoft.MvvmLight.Command;
using Galasoft.MvvmLight.CommandWpf;

// Toolkit.Mvvm
using Microsoft.Toolkit.Mvvm.Input;
```

### Using RelayCommand with asynchronous actions

If you are currently using the MvvmLight `RelayCommand<T>` implementation with asynchronous actions, the MVVM Toolkit exposes an improved implementation for these scenarios. 

You can simply replace your existing `RelayCommand<T>` with the `AsyncRelayCommand<T>` which has been built for asynchronous purposes.

```csharp
// MvvmLight
var command = new RelayCommand<string>(async () => await OnCommandAsync());

// Toolkit.Mvvm
var asyncCommand = new AsyncRelayCommand<string>(OnCommandAsync);
```

### RelayCommand<T> Methods

#### RaiseCanExecuteChanged()

The functionality of `RaiseCanExecuteChanged()` can be achieved with the MVVM Toolkit's `NotifyCanExecuteChanged()` method. 

```csharp
// MvvmLight
var command = new RelayCommand<string>(OnCommand);
command.RaiseCanExecuteChanged();

// Toolkit.Mvvm
var command = new RelayCommand<string>(OnCommand);
command.NotifyCanExecuteChanged();
```

## Migrating SimpleIoc

The [IoC](Ioc.md) implementation in the MVVM Toolkit doesn't include any built-in logic to handle dependency injection on its own, so you're free to use any 3rd party library to retrieve an `IServiceProvider` instance that you can then pass to the `Ioc.ConfigureServices` method. In the examples below, the `ServiceCollection` type from the `Microsoft.Extensions.DependencyInjection` library will be used.

This is the biggest change between MvvmLight and the MVVM Toolkit. 

This implementation will feel familiar if you've implemented dependency injection with ASP.NET Core applications.

### Registering your dependencies

With MvvmLight, you may have registered your dependencies similar to these scenarios using `SimpleIoc`.

```csharp
public void RegisterServices()
{
  SimpleIoc.Default.Register<INavigationService, NavigationService>();

  SimpleIoc.Default.Register<IDialogService>(() => new DialogService());
}
```

With the MVVM Toolkit, you would achieve the same as follows.

```csharp
public void RegisterServices()
{
  Ioc.Default.ConfigureServices(
    new ServiceCollection()
    .AddSingleton<INavigationService, NavigationService>()
    .AddSingleton<IDialogService>(new DialogService())
    .BuildServiceProvider());
}
```

### Resolving dependencies

Once initialized, services can be retrieved from the `Ioc` class just like with `SimpleIoc`:

```csharp
IDialogService dialogService = SimpleIoc.Default.GetInstance<IDialogService>();
```

Migrating to the MVVM Toolkit, you will achieve the same with:

```csharp
IDialogService dialogService = Ioc.Default.GetService<IDialogService>();
```

### Removing dependencies

With `SimpleIoc`, you would unregister your dependencies with the following method call.

```csharp
SimpleIoc.Default.Unregister<INavigationService>();
```

There is no direct replacement for removing dependencies with the MVVM Toolkit `Ioc` implementation.

### Preferred constructor

When registering your dependencies with MvvmLight's `SimpleIoc`, you have the option in your classes to provide a `PreferredConstructor` attribute for those with multiple constructors.

This attribute will need removing where used, and you will need to use any attributes from the 3rd party dependency injection library in use, if supported.

## Migrating Messenger

The following steps focus on migrating your existing components which take advantage of the `Messenger` of the MvvmLight Toolkit. 

The Windows Community Toolkit MVVM framework provides two messenger implementations (`WeakReferenceMessenger` and `StrongReferenceMessenger`, see [docs here](Messenger.md)) that provides similar functionality, with some key differences detailed below.

Below are a list of migrations that will need to be performed if being used in your current solution.

The first change here will be swapping using directives in your components.

```csharp
// MvvmLight
using GalaSoft.MvvmLight.Messaging;

// Toolkit.Mvvm
using Microsoft.Toolkit.Mvvm.Messaging;
```

### Messenger methods

#### Register<TMessage>(object, Action<TMessage>)

The functionality of `Register<TMessage>(object, Action<TMessage>)` can be achieved with the MVVM Toolkit's `IMessenger` extension method `Register<TRecipient, TMessage>(object, MessageHandler<TRecipient, TMessage>)`.

```csharp
// MvvmLight
Messenger.Default.Register<MyMessage>(this, this.OnMyMessageReceived);

// Toolkit.Mvvm
Messenger.Register<MyViewModel, MyMessage>(this, static (r, m) => r.OnMyMessageReceived(m));
```

The reason for this signature is that it allows the messenger to use weak references to properly track recipients and to avoid creating closures to capture the recipient itself. That is, the input recipient is passed as an input to the lambda expression, so it doesn't need to be captured by the lambda expression itself. This also results in more efficient code, as the same handler can be reused multiple times with no allocations. Note that this is just one of the supported ways to register handlers, and it is possible to also use the `IRecipient<TMessage>` interface instead (detailed [in the messenger docs](Messenger.md)), which makes the registration automatic and less verbose.

> [!NOTE]
> The `static` modifier for lambda expressions requires C# 9, and it is optional. It is useful to use it here to ensure you're not accidentally capturing the recipient or some other member, hence causing the allocation of a closure, but it is not mandatory. If you can't use C# 9, you can just remove `static` here and just be careful to ensure the code is not capturing anything.

Additionally, this example and the ones below will just be using the `Messenger` property from `ObservableRecipient`. If you want to just statically access a messenger instance from anywhere else in your code, the same examples apply as well, with the only difference being that `Messenger` needs to be replaced with eg. `WeakReferenceMessenger.Default` instead.

#### Register<TMessage>(object, bool, Action<TMessage>)

There is no direct replacement for this registration mechanism which allows you to support receiving messages for derived message types also. This change is intentional as the `Messenger` implementation aims to not use reflection to achieve its performance benefits.

Alternatively, there are a few options that can be done to achieve this functionality. 

- Create a custom `IMessenger` implementation.
- Register the additional message types using a shared handler than then checks the type and invokes the right method.

```csharp
// MvvmLight
Messenger.Default.Register<MyMessage>(this, true, this.OnMyMessageReceived);

// Toolkit.Mvvm
Messenger.Register<MyViewModel, MyMessage>(this, static (r, m) => r.OnMyMessageReceived(m));
Messenger.Register<MyViewModel, MyOtherMessage>(this, static (r, m) => r.OnMyMessageReceived(m));
```

#### Register<TMessage>(object, object, Action<TMessage>)

The functionality of `Register<TMessage>(object, object, Action<TMessage>)` can be achieved with the MVVM Toolkit's `Register<TRecipient, TMessage, TToken>(object, TToken, MessageHandler<TRecipient, TMessage>)` method. 

```csharp
// MvvmLight
Messenger.Default.Register<MyMessage>(this, nameof(MyViewModel), this.OnMyMessageReceived);

// Toolkit.Mvvm
Messenger.Register<MyViewModel, MyMessage, string>(this, nameof(MyViewModel), static (r, m) => r.OnMyMessageReceived(m));
```

#### Register<TMessage>(object, object, bool, Action<TMessage>)

There is no direct replacement for this registration mechanism which allows you to support receiving messages for derived message types also. This change is intentional as the `Messenger` implementation aims to not use reflection to achieve its performance benefits.

Alternatively, there are a few options that can be done to achieve this functionality. 

- Create a custom `IMessenger` implementation.
- Register the additional message types using a shared handler than then checks the type and invokes the right method.

```csharp
// MvvmLight
Messenger.Default.Register<MyMessage>(this, nameof(MyViewModel), true, this.OnMyMessageReceived);

// Toolkit.Mvvm
Messenger.Register<MyViewModel, MyMessage, string>(this, nameof(MyViewModel), static (r, m) => r.OnMyMessageReceived(m));
Messenger.Register<MyViewModel, MyOtherMessage, string>(this, nameof(MyViewModel), static (r, m) => r.OnMyMessageReceived(m));
```

#### Send<TMessage>(TMessage)

The functionality of `Send<TMessage>(TMessage)` can be achieved with the MVVM Toolkit's `IMessenger` extension method `Send<TMessage>(TMessage)`. 

```csharp
// MvvmLight
Messenger.Default.Send<MyMessage>(new MyMessage());
Messenger.Default.Send(new MyMessage());

// Toolkit.Mvvm
Messenger.Send(new MyMessage());
```

In the above scenario where the message being sent has a parameterless constructor, the MVVM Toolkit has a simplified extension to send a message in this format.

```csharp
// Toolkit.Mvvm
Messenger.Send<MyMessage>();
```

#### Send<TMessage>(TMessage, object)

The functionality of `Send<TMessage>(TMessage, object)` can be achieved with the MVVM Toolkit's `Send<TMessage, TToken>(TMessage, TToken)` method. 

```csharp
// MvvmLight
Messenger.Default.Send<MyMessage>(new MyMessage(), nameof(MyViewModel));
Messenger.Default.Send(new MyMessage(), nameof(MyViewModel));

// Toolkit.Mvvm
Messenger.Send(new MyMessage(), nameof(MyViewModel));
```

#### Unregister(object)

The functionality of `Unregister(object)` can be achieved with the MVVM Toolkit's `UnregisterAll(object)` method. 

```csharp
// MvvmLight
Messenger.Default.Unregister(this);

// Toolkit.Mvvm
Messenger.UnregisterAll(this);
```

#### Unregister<TMessage>(object)

The functionality of `Unregister<TMessage>(object)` can be achieved with the MVVM Toolkit's `IMessenger` extension method `Unregister<TMessage>(object)`. 

```csharp
// MvvmLight
Messenger.Default.Unregister<MyMessage>(this);

// Toolkit.Mvvm
Messenger.Unregister<MyMessage>(this);
```

#### Unregister<TMessage>(object, Action<TMessage>)

There is no direct replacement for the `Unregister<TMessage>(object, Action<TMessage>)` method in the MVVM Toolkit. 

The reason for the omission is that a message recipient can only have a single registered handler for any given message type. 

We recommend achieving this functionality with the MVVM Toolkit's `IMessenger` extension method `Unregister<TMessage>(object)`. 

```csharp
// MvvmLight
Messenger.Default.Unregister<MyMessage>(this, OnMyMessageReceived);

// Toolkit.Mvvm
Messenger.Unregister<MyMessage>(this);
```

#### Unregister<TMessage>(object, object)

The functionality of `Unregister<TMessage>(object, object)` can be achieved with the MVVM Toolkit's `Unregister<TMessage, TToken>(object, TToken)` method. 

```csharp
// MvvmLight
Messenger.Default.Unregister<MyMessage>(this, nameof(MyViewModel));

// Toolkit.Mvvm
Messenger.Unregister<MyMessage, string>(this, nameof(MyViewModel));
```

#### Unregister<TMessage>(object, object, Action<TMessage>)

There is no direct replacement for the `Unregister<TMessage>(object, object, Action<TMessage>)` method in the MVVM Toolkit. 

The reason for the omission is that a message recipient can only have a single registered handler for any given message type. 

We recommend achieving this functionality with the MVVM Toolkit's `Unregister<TMessage, TToken>(object, TToken)` method. 

```csharp
// MvvmLight
Messenger.Default.Unregister<MyMessage>(this, nameof(MyViewModel), OnMyMessageReceived);

// Toolkit.Mvvm
Messenger.Unregister<MyMessage, string>(this, nameof(MyViewModel));
```

#### Cleanup()

The `Cleanup` method has a direct replacement with the same name in the MVVM Toolkit. Note that this method is only useful when a messenger using weak references is being used, while the `StrongReferenceMessenger` type will simply do nothing when this method is called, as the internal state is already trimmed automatically as the messenger is being used.

```csharp
// MvvmLight
Messenger.Default.Cleanup();

// Toolkit.Mvvm
Messenger.Cleanup();
```

#### RequestCleanup()

There is no direct replacement for the `RequestCleanup` method in the MVVM Toolkit. In the context of MvvmLight, `RequestCleanup` is used to initiate a request to remove registrations which are no longer alive as the implementation takes advantage of weak references. 

Any calls to the `RequestCleanup` method can be removed or replaced with `Cleanup`.

```csharp
// MvvmLight
Messenger.Default.RequestCleanup();

// Toolkit.Mvvm
// No direct replacement, remove
```

#### ResetAll()

The functionality of `ResetAll()` can be achieved with the MVVM Toolkit's `Reset()` method. 

Unlike MvvmLight's implementation which nulls out the instance, the MVVM Toolkit clears the registered maps. 

```csharp
// MvvmLight
Messenger.Default.ResetAll();

// Toolkit.Mvvm
Messenger.Reset();
```

### Messenger static methods

#### OverrideDefault(IMessenger)

There is no direct replacement for the `OverrideDefault(IMessenger)` method in the MVVM Toolkit.

To use a custom implementation of the `IMessenger`, either registered the custom implementation in the service registrations for dependency injection or manually construct a static instance and pass this where required. 

```csharp
// MvvmLight
Messenger.OverrideDefault(new Messenger());

// Toolkit.Mvvm
// No direct replacement
```

#### Reset()

There is no direct replacement for the static `Reset` method in the MVVM Toolkit.

The same functionality can be achieved by calling the `Reset` method of the static `Default` instance of one of the messenger types.

```csharp
// MvvmLight
Messenger.Reset();

// Toolkit.Mvvm
WeakReferenceMessenger.Default.Reset();
```

### Messenger Static Properties

#### Default

`Default` has a direct replacement, `Default`, requiring no change to your existing implementation.

```csharp
// MvvmLight
IMessenger messenger = Messenger.Default;

// Toolkit.Mvvm
IMessenger messenger = WeakReferenceMessenger.Default;
```

## Migrating message types

The message types provided in the MvvmLight toolkit are designed as a base for you as a developer to work with if needed. 

While the MVVM Toolkit provides some alternatives, there are no direct replacement for these message types. We recommend looking at our [available message types](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Mvvm/Messaging/Messages).

Alternatively, if your solution takes advantage of the MvvmLight message types, these can easily be ported into your own codebase.

## Migrating platform-specific components

In the current MVVM Toolkit implementation, there are no replacements for platform-specific components which exist in the MvvmLight toolkit. 

The following components and their associated helpers/extension methods do not have a replacement and will need considering when migrating to the MVVM Toolkit.

### Android/iOS/Windows specific

- DialogService
- DispatcherHelper
- NavigationService

### Android/iOS specific

- ActivityBase
- Binding
- BindingMode
- PropertyChangedEventManager
- UpdateTriggerMode

### Android specific

- CachingViewHolder
- ObservableAdapter
- ObservableRecyclerAdapter

### iOS specific

- ObservableCollectionViewSource
- ObservableTableViewController
- ObservableTableViewSource

### Helpers

- Empty
- WeakAction
- WeakFunc
