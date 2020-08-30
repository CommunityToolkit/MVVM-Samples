---
title: Migrating from MvvmLight
author: jamesmcroft
description: This article describes how to migrate MvvmLight solutions to the Windows Community Toolkit MVVM framework.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, mvvmlight, net core, net standard
dev_langs:
  - csharp
---

# Migrating from MvvmLight

This article outlines some of the key differences between the [MvvmLight Toolkit](https://github.com/lbugnion/mvvmlight) and the MVVM Toolkit to ease your migration. 

While this article specifically focuses on the migrations from MvvmLight to the MVVM Toolkit, note that there are additional improvements that have been made within the MVVM Toolkit so I highly recommend taking a look at the documentation for the APIs. 

## Installing the WCT MVVM Toolkit

To take advantage of the Windows Community Toolkit MVVM framework, you'll first need to install the latest NuGet package to your existing Windows application.

### Install via .NET CLI

```
dotnet add package Microsoft.Toolkit.Mvvm --version x.x.x
```

### Install via PackageReference

```
<PackageReference Include="Microsoft.Toolkit.Mvvm" Version="x.x.x" />
```

## Migrating ObservableObject

The following steps focus on migrating your existing components which take advantage of the `ObservableObject` of the MvvmLight Toolkit. 

The Windows Community Toolkit MVVM framework provides an [`ObservableObject`](ObservableObject) type that is similar. 

The first change here will be swapping using directives in your components.

```csharp
// MvvmLight
using GalaSoft.MvvmLight;

// Toolkit.Mvvm
using Microsoft.Toolkit.Mvvm.ComponentModel;
```

Below are a list of migrations that will need to be performed if being used in your current solution.

### ObservableObject Methods

#### Set<T> ( Expression, ref T, T )

`Set(Expression, ref T, T)` does not have a like-for-like method signature replacement. 

However, `SetProperty(ref T, T, string)` provides the same functionality with additional performance benefits. 

```csharp
// MvvmLight
this.Set(() => this.MyProperty, ref this.myProperty, value);

// Toolkit.Mvvm
this.SetProperty(ref this.myProperty, value, nameof(this.MyProperty));
```

Note, string parameter is not required if the method is being called from the property's setter as it is inferred from the caller member name.

#### Set<T> ( string, ref T, T )

`Set<T>(string, ref T, T)` does not have a like-for-like method signature replacement. 

However, `SetProperty<T>(ref T, T, string)` provides the same functionality with re-ordered parameters.

```csharp
// MvvmLight
this.Set(nameof(this.MyProperty), ref this.myProperty, value);

// Toolkit.Mvvm
this.SetProperty(ref this.myProperty, value, nameof(this.MyProperty));
```

Note, string parameter is not required if the method is being called from the property's setter as it is inferred from the caller member name.

#### Set<T> ( ref T, T, string )

`Set<T>(ref T, T, string)` has a renamed direct replacement, `SetProperty<T>(ref T, T, string)`.

```csharp
// MvvmLight
this.Set(ref this.myProperty, value, nameof(this.MyProperty));

// Toolkit.Mvvm
this.SetProperty(ref this.myProperty, value, nameof(this.MyProperty));
```

Note, string parameter is not required if the method is being called from the property's setter as it is inferred from the caller member name.

#### RaisePropertyChanged ( string )

`RaisePropertyChanged(string)` has a renamed direct replacement, `OnPropertyChanged(string)`.

```csharp
// MvvmLight
this.RaisePropertyChanged(nameof(this.MyProperty));

// Toolkit.Mvvm
this.OnPropertyChanged(nameof(this.MyProperty));
```

Note, string parameter is not required if the method is being called from the property's setter as it is inferred from the caller member name.

#### RaisePropertyChanged<T> ( Expression )

`RaisePropertyChanged<T>(Expression)` does not have a direct replacement. 

It is recommended for improved performance that you replace `RaisePropertyChanged<T>(Expression)` with the Toolkit's `OnPropertyChanged(string)` using the `nameof` keyword instead.

```csharp
// MvvmLight
this.RaisePropertyChanged(() => this.MyProperty);

// Toolkit.Mvvm
this.OnPropertyChanged(nameof(this.MyProperty));
```

#### VerifyPropertyName ( string )

There is no direct replacement for the `VerifyPropertyName(string)` method and any code using this should be altered or removed.

The reason for the omission from the MVVM Toolkit is that using the `nameof` keyword for a property verifies that it exists. When MvvmLight was built, the `nameof` keyword was not available and this method was used to ensure that the property existed on the object.

```csharp
// MvvmLight
this.VerifyPropertyName(nameof(this.MyProperty));

// Toolkit.Mvvm
// No direct replacement, remove
```

### ObservableObject Properties

#### PropertyChangedHandler

`PropertyChangedHandler` does not have a direct replacement.

To raise a property changed event via the `PropertyChanged` event handler, you need to call the `OnPropertyChanged` method instead. 

```csharp
// MvvmLight
PropertyChangedEventHandler handler = this.PropertyChangedHandler;

// Toolkit.Mvvm
this.OnPropertyChanged(nameof(this.MyProperty));
```

## Migrating ViewModelBase

The following steps focus on migrating your existing components which take advantage of the `ViewModelBase` of the MvvmLight Toolkit. 

The Windows Community Toolkit MVVM framework provides an [`ObservableRecipient`](ObservableRecipient) type that provides similar functionality. 

Below are a list of migrations that will need to be performed if being used in your current solution.

### ViewModelBase Methods

#### Set<T> ( string, ref T, T, bool )

`Set<T>(string, ref T, T, bool)` does not have a like-for-like method signature replacement. 

However, `SetProperty<T>(ref T, T, bool, string)` provides the same functionality with re-ordered parameters.

```csharp
// MvvmLight
this.Set(nameof(this.MyProperty), ref this.myProperty, value, true);

// Toolkit.Mvvm
this.SetProperty(ref this.myProperty, value, true, nameof(this.MyProperty));
```

Note, the value and broadcast boolean parameters are not optional in the Toolkit's implementation and must be provided to use this method. The reason for this change is that by omitting the broadcast parameter when calling this method, it will by default call the ObservableObject's `SetProperty` method.

Also, string parameter is not required if the method is being called from the property's setter as it is inferred from the caller member name.

#### Set<T> ( ref T, T, bool, string )

`Set<T>(ref T, T, bool, string)` has a renamed direct replacement, `SetProperty<T>(ref T, T, bool, string)`. 

```csharp
// MvvmLight
this.Set(ref this.myProperty, value, true, nameof(this.MyProperty));

// Toolkit.Mvvm
this.SetProperty(ref this.myProperty, value, true, nameof(this.MyProperty));
```

Note, string parameter is not required if the method is being called from the property's setter as it is inferred from the caller member name.

#### Set<T> ( Expression, ref T, T, bool )

`Set<T>(Expression, ref T, T, bool)` does not have a direct replacement. 

It is recommended for improved performance that you replace this with the Toolkit's `SetProperty<T>(ref T, T, bool, string)` using the `nameof` keyword instead.

```csharp
// MvvmLight
this.Set<MyObject>(() => this.MyProperty, ref this.myProperty, value, true);

// Toolkit.Mvvm
this.SetProperty<MyObject>(ref this.myProperty, value, true, nameof(this.MyProperty));
```

Note, string parameter is not required if the method is being called from the property's setter as it is inferred from the caller member name.

#### Broadcast<T> ( T, T, string )

`Broadcast<T>(T, T, string)` has a direct replacement which doesn't require a rename. 

```csharp
// MvvmLight
this.Broadcast<MyObject>(this.oldValue, this.newValue, nameof(this.MyProperty));

// Toolkit.Mvvm
this.Broadcast<MyObject>(this.oldValue, this.newValue, nameof(this.MyProperty));
```

Note, the message sent via the `Messenger` property when calling the `Broadcast` method has a direct replacement for `PropertyChangedMessage` within the Toolkit's framework. 

Also, string parameter is not required if the method is being called from the property's setter as it is inferred from the caller member name.

#### RaisePropertyChanged<T> ( string, T, T, bool )

There is no direct replacement for the `RaisePropertyChanged<T>(string, T, T, bool)` method. 

The simplest alternative is to call `OnPropertyChanged` and subsequently call `Broadcast` to achieve this functionality.

```csharp
// MvvmLight
this.RaisePropertyChanged<MyObject>(nameof(this.MyProperty), this.oldValue, this.newValue, true);

// Toolkit.Mvvm
this.OnPropertyChanged<MyObject>(nameof(this.MyProperty));
this.Broadcast<MyObject>(this.oldValue, this.newValue, nameof(this.MyProperty));
```

#### RaisePropertyChanged<T> ( Expression, T, T, bool )

There is no direct replacement for the `RaisePropertyChanged<T>(Expression, T, T, bool)` method.

The simplest alternative is to call `OnPropertyChanged` and subsequently call `Broadcast` to achieve this functionality.

```csharp
// MvvmLight
this.RaisePropertyChanged<MyObject>(() => this.MyProperty, this.oldValue, this.newValue, true);

// Toolkit.Mvvm
this.OnPropertyChanged<MyObject>(nameof(this.MyProperty));
this.Broadcast<MyObject>(this.oldValue, this.newValue, nameof(this.MyProperty));
```

#### ICleanup.Cleanup ()

There is no direct replacement for the `ICleanup` interface.

However, the `ObservableRecipient` provides an `OnDeactivated` method which should be used to provide the same functionality as `Cleanup`. 

`OnDeactivated` in the MVVM Toolkit will also unregister all of the registered messenger events when called.

```csharp
// MvvmLight
this.Cleanup();

// Toolkit.Mvvm
this.OnDeactivated();
```

Note, the `OnActivated` and `OnDeactivated` methods can be called from your existing solution as with `Cleanup`. 

However, the `ObservableRecipient` exposes an `IsActive` property that also controls the call to these methods when it is set. 

### ViewModelBase Properties

#### MessengerInstance

`MessengerInstance` has a renamed direct replacement, `Messenger`.

```csharp
// MvvmLight
IMessenger messenger = this.MessengerInstance;

// Toolkit.Mvvm
IMessenger messenger = this.Messenger;
```

#### IsInDesignMode

There is no direct replacement for the `IsInDesignMode` property and any code using this should be altered or removed.

The reason for the omission from the MVVM Toolkit is that the `IsInDesignMode` property exposed platform-specific implementations. The MVVM Toolkit has been designed to be platform agnostic.

```csharp
// MvvmLight
var isInDesignMode = this.IsInDesignMode;

// Toolkit.Mvvm
// No direct replacement, remove
```

### ViewModelBase Static Properties

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

The Windows Community Toolkit MVVM framework provides a [`RelayCommand`](RelayCommand) type that provides like-for-like functionality taking advantage of the `ICommand` System interface. 

Below are a list of migrations that will need to be performed if being used in your current solution. Where a method or property isn't listed, there is a direct replacement with the same name in the MVVM Toolkit and there is no change required.

The first change here will be swapping using directives in your components.

```csharp
// MvvmLight
using GalaSoft.MvvmLight.Command;
using Galasoft.MvvmLight.CommandWpf;

// Toolkit.Mvvm
using Microsoft.Toolkit.Mvvm.Input;
```

**Note** on `RelayCommand` constructors. MvvmLight uses weak references to establish the link between the command and the action called from the associated class. This is not required by the MVVM Toolkit implementation and if this optional parameter has been set to `true` in any of your constructors, this will be removed.

### Using RelayCommand with asynchronous actions

If you are currently using the MvvmLight `RelayCommand` implementation with asynchronous actions, the MVVM Toolkit exposes an improved implementation for these scenarios. 

You can simply replace your existing `RelayCommand` with the `AsyncRelayCommand` which has been built for asynchronous purposes.

```csharp
// MvvmLight
var command = new RelayCommand(() => this.OnCommandAsync());
var command = new RelayCommand(async () => await this.OnCommandAsync());

// Toolkit.Mvvm
var asyncCommand = new AsyncRelayCommand(this.OnCommandAsync);
```

### RelayCommand Methods

#### RaiseCanExecuteChanged ()

The functionality of `RaiseCanExecuteChanged()` can be achieved with the MVVM Toolkit's `NotifyCanExecuteChanged()` method. 

```csharp
// MvvmLight
var command = new RelayCommand(this.OnCommand);
command.RaiseCanExecuteChanged();

// Toolkit.Mvvm
var command = new RelayCommand(this.OnCommand);
command.NotifyCanExecuteChanged();
```

## Migrating RelayCommand<T>

The following steps focus on migrating your existing components which take advantage of the `RelayCommand<T>` of the MvvmLight Toolkit. 

The Windows Community Toolkit MVVM framework provides a [`RelayCommand<T>`](RelayCommand) type that provides like-for-like functionality taking advantage of the `ICommand` System interface. 

Below are a list of migrations that will need to be performed if being used in your current solution. Where a method or property isn't listed, there is a direct replacement with the same name in the MVVM Toolkit and there is no change required.

The first change here will be swapping using directives in your components.

```csharp
// MvvmLight
using GalaSoft.MvvmLight.Command;
using Galasoft.MvvmLight.CommandWpf;

// Toolkit.Mvvm
using Microsoft.Toolkit.Mvvm.Input;
```

**Note** on `RelayCommand<T>` constructors. MvvmLight uses weak references to establish the link between the command and the action called from the associated class. This is not required by the MVVM Toolkit implementation and if this optional parameter has been set to `true` in any of your constructors, this will be removed.

### Using RelayCommand with asynchronous actions

If you are currently using the MvvmLight `RelayCommand<T>` implementation with asynchronous actions, the MVVM Toolkit exposes an improved implementation for these scenarios. 

You can simply replace your existing `RelayCommand<T>` with the `AsyncRelayCommand<T>` which has been built for asynchronous purposes.

```csharp
// MvvmLight
var command = new RelayCommand<string>(async () => await this.OnCommandAsync());

// Toolkit.Mvvm
var asyncCommand = new AsyncRelayCommand<string>(this.OnCommandAsync);
```

### RelayCommand<T> Methods

#### RaiseCanExecuteChanged ()

The functionality of `RaiseCanExecuteChanged()` can be achieved with the MVVM Toolkit's `NotifyCanExecuteChanged()` method. 

```csharp
// MvvmLight
var command = new RelayCommand<string>(this.OnCommand);
command.RaiseCanExecuteChanged();

// Toolkit.Mvvm
var command = new RelayCommand<string>(this.OnCommand);
command.NotifyCanExecuteChanged();
```

## Migrating SimpleIoc

The [IoC](Ioc) implementation in the MVVM Toolkit takes advantage of existing .NET APIs through the `Microsoft.Extensions.DependencyInjection` library. 

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
  Ioc.Default.ConfigureServices(services => 
  {
    services.AddSingleton<INavigationService, NavigationService>();

    services.AddSingleton<IDialogService>(new DialogService());
  });
}
```

### Resolving dependencies

Ioc in the MVVM Toolkit, like many other service providers, is capable of constructor injection. 

There are also often times in your application where you need to access the services from the service itself.

In MvvmLight, you might access a service directly as follows:

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

This attribute will need removing where used. 

The dependency injection within the MVVM Toolkit's `Ioc` determines the best constructor based on the dependencies that have been added to the collection.

The example below shows a dependency that relies on other dependencies with multiple constructors.

```csharp
public class NavigationService : INavigationService
{
    public NavigationService()
    {
    }

    public NavigationService(IDialogService dialogService) : this(null, dialogService)
    {
    }

    public NavigationService(IToastService toastService, IDialogService dialogService)
    {
    }
}
```

With your dependencies registered as below, the `NavigationService(IToastService, IDialogService)` constructor will be called as both of the required dependencies are registered also.

```csharp
Ioc.Default.ConfigureServices(services =>
{
    services.AddSingleton<IDialogService, DialogService>();
    services.AddSingleton<IToastService, ToastService>();
    services.AddSingleton<INavigationService, NavigationService>();
});
```

## Migrating Messenger

The following steps focus on migrating your existing components which take advantage of the `Messenger` of the MvvmLight Toolkit. 

The Windows Community Toolkit MVVM framework provides a [`Messenger`](Messenger) type that provides similar functionality. 

A note on `Register` methods throughout this migration. MvvmLight uses weak references to establish the link between the `Messenger` instance and the recipient. This is not required by the MVVM Toolkit implementation and if this optional parameter has been set to `true` in any of your `Register` method calls, this will be removed.

Below are a list of migrations that will need to be performed if being used in your current solution.

The first change here will be swapping using directives in your components.

```csharp
// MvvmLight
using GalaSoft.MvvmLight.Messaging;

// Toolkit.Mvvm
using Microsoft.Toolkit.Mvvm.Messaging;
```

### Messenger Methods

#### Register<TMessage> ( object, Action<TMessage> )

The functionality of `Register<TMessage>(object, Action<TMessage>)` can be achieved with the MVVM Toolkit's `IMessenger` extension method `Register<TMessage>(object, Action<TMessage>)`. 

```csharp
// MvvmLight
Messenger.Default.Register<MyMessage>(this, this.OnMyMessageReceived);

// Toolkit.Mvvm
Messenger.Default.Register<MyMessage>(this, this.OnMyMessageReceived);
```

#### Register<TMessage> ( object, bool, Action<TMessage> )

There is no direct replacement for this registration mechanism which allows you to support receiving messages for derived message types also. This change is intentional as the `Messenger` implementation aims to not use reflection to achieve its performance benefits.

Alternatively, there are a few options that can be done to achieve this functionality. 

- Create a custom `IMessenger` implementation
- Register the additional message types using a shared `Action`

```csharp
// MvvmLight
Messenger.Default.Register<MyMessage>(this, true, this.OnMyMessageReceived);

// Toolkit.Mvvm
Messenger.Default.Register<MyMessage, string>(this, nameof(MyViewModel), this.OnMyMessageReceived);
Messenger.Default.Register<MyOtherMessage, string>(this, nameof(MyViewModel), this.OnMyMessageReceived);
```

#### Register<TMessage> ( object, object, Action<TMessage> )

The functionality of `Register<TMessage>(object, object, Action<TMessage>)` can be achieved with the MVVM Toolkit's `Register<TMessage, TToken>(object, TToken, Action<TMessage>)` method. 

```csharp
// MvvmLight
Messenger.Default.Register<MyMessage>(this, nameof(MyViewModel), this.OnMyMessageReceived);

// Toolkit.Mvvm
Messenger.Default.Register<MyMessage, string>(this, nameof(MyViewModel), this.OnMyMessageReceived);
```

#### Register<TMessage> ( object, object, bool, Action<TMessage> )

There is no direct replacement for this registration mechanism which allows you to support receiving messages for derived message types also. This change is intentional as the `Messenger` implementation aims to not use reflection to achieve its performance benefits.

Alternatively, there are a few options that can be done to achieve this functionality. 

- Create a custom `IMessenger` implementation
- Register the additional message types using a shared `Action`

```csharp
// MvvmLight
Messenger.Default.Register<MyMessage>(this, nameof(MyViewModel), true, this.OnMyMessageReceived);

// Toolkit.Mvvm
Messenger.Default.Register<MyMessage, string>(this, nameof(MyViewModel), this.OnMyMessageReceived);
Messenger.Default.Register<MyOtherMessage, string>(this, nameof(MyViewModel), this.OnMyMessageReceived);
```

#### Send<TMessage> ( TMessage )

The functionality of `Send<TMessage>(TMessage)` can be achieved with the MVVM Toolkit's `IMessenger` extension method `Send<TMessage>(TMessage)`. 

```csharp
// MvvmLight
Messenger.Default.Send<MyMessage>(new MyMessage());
Messenger.Default.Send(new MyMessage());

// Toolkit.Mvvm
Messenger.Default.Send<MyMessage>(new MyMessage());
Messenger.Default.Send(new MyMessage());
```

In the above scenario where the message being sent has a parameterless constructor, the MVVM Toolkit has a simplified extension to send a message in this format.

```csharp
// Toolkit.Mvvm
Messenger.Default.Send<MyMessage>();
```

#### Send<TMessage> ( TMessage, object )

The functionality of `Send<TMessage>(TMessage, object)` can be achieved with the MVVM Toolkit's `Send<TMessage, TToken>(TMessage, TToken)` method. 

```csharp
// MvvmLight
Messenger.Default.Send<MyMessage>(new MyMessage(), nameof(MyViewModel));
Messenger.Default.Send(new MyMessage(), nameof(MyViewModel));

// Toolkit.Mvvm
Messenger.Default.Send(new MyMessage(), nameof(MyViewModel));
```

#### Unregister ( object )

The functionality of `Unregister(object)` can be achieved with the MVVM Toolkit's `UnregisterAll(object)` method. 

```csharp
// MvvmLight
Messenger.Default.Unregister(this);

// Toolkit.Mvvm
Messenger.Default.UnregisterAll(this);
```

#### Unregister<TMessage> ( object )

The functionality of `Unregister<TMessage>(object)` can be achieved with the MVVM Toolkit's `IMessenger` extension method `Unregister<TMessage>(object)`. 

```csharp
// MvvmLight
Messenger.Default.Unregister<MyMessage>(this);

// Toolkit.Mvvm
Messenger.Default.Unregister<MyMessage>(this);
```

#### Unregister<TMessage> ( object, Action<TMessage> )

There is no direct replacement for the `Unregister<TMessage>(object, Action<TMessage>)` method in the MVVM Toolkit. 

Alternatively, we recommend achieving a similar functionality with the MVVM Toolkit's `IMessenger` extension method `Unregister<TMessage>(object)`. 

```csharp
// MvvmLight
Messenger.Default.Unregister<MyMessage>(this, this.OnMyMessageReceived);

// Toolkit.Mvvm
Messenger.Default.Unregister<MyMessage>(this);
```

#### Unregister<TMessage> ( object, object )

The functionality of `Unregister<TMessage>(object, object)` can be achieved with the MVVM Toolkit's `Unregister<TMessage, TToken>(object, TToken)` method. 

```csharp
// MvvmLight
Messenger.Default.Unregister<MyMessage>(this, nameof(MyViewModel));

// Toolkit.Mvvm
Messenger.Default.Unregister<MyMessage, string>(this, nameof(MyViewModel));
```

#### Unregister<TMessage> ( object, object, Action<TMessage> )

There is no direct replacement for the `Unregister<TMessage>(object, object, Action<TMessage>)` method in the MVVM Toolkit. 

Alternatively, we recommend achieving a similar functionality with the MVVM Toolkit's `Unregister<TMessage, TToken>(object, TToken)` method. 

```csharp
// MvvmLight
Messenger.Default.Unregister<MyMessage>(this, nameof(MyViewModel), this.OnMyMessageReceived);

// Toolkit.Mvvm
Messenger.Default.Unregister<MyMessage, string>(this, nameof(MyViewModel));
```

#### Cleanup ()

There is no direct replacement for the `Cleanup` method in the MVVM Toolkit. In the context of MvvmLight, `Cleanup` is used to remove registrations which are no longer alive as the implementation takes advantage of weak references. 

Any calls to the `Cleanup` method can be removed.

```csharp
// MvvmLight
Messenger.Default.Cleanup();

// Toolkit.Mvvm
// No direct replacement, remove
```

#### RequestCleanup ()

There is no direct replacement for the `RequestCleanup` method in the MVVM Toolkit. In the context of MvvmLight, `RequestCleanup` is used to initiate a request to remove registrations which are no longer alive as the implementation takes advantage of weak references. 

Any calls to the `RequestCleanup` method can be removed.

```csharp
// MvvmLight
Messenger.Default.RequestCleanup();

// Toolkit.Mvvm
// No direct replacement, remove
```

#### ResetAll ()

There is no direct replacement for the `ResetAll` method in the MVVM Toolkit. In the context of MvvmLight, this method is used to reset the default `IMessenger` instance by calling the static `Reset` method.

Any calls to the `ResetAll()` method can be removed.

```csharp
// MvvmLight
Messenger.Default.ResetAll();

// Toolkit.Mvvm
// No direct replacement, remove
```

### Messenger Static Methods

#### OverrideDefault ( IMessenger )

There is no direct replacement for the `OverrideDefault(IMessenger)` method in the MVVM Toolkit.

Any calls to the `Messenger.OverrideDefault(IMessenger)` method can be removed.

```csharp
// MvvmLight
Messenger.OverrideDefault(new Messenger());

// Toolkit.Mvvm
// No direct replacement, remove
```

#### Reset ()

There is no direct replacement for the `Reset` method in the MVVM Toolkit. In the context of MvvmLight, this static method is used to reset the default `IMessenger` instance.

Any calls to the `Messenger.Reset()` method can be removed.

```csharp
// MvvmLight
Messenger.Reset();

// Toolkit.Mvvm
// No direct replacement, remove
```

### Messenger Static Properties

#### Default

`Default` has a direct replacement, `Default`, requiring no change to your existing implementation.

```csharp
// MvvmLight
IMessenger messenger = Messenger.Default;

// Toolkit.Mvvm
IMessenger messenger = Messenger.Default;
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