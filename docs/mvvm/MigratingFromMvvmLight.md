---
title: Migrating from MvvmLight
author: jamesmcroft
description: This article describes how to migrate MvvmLight solutions to the Windows Community Toolkit MVVM framework.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, mvvmlight, net core, net standard
dev_langs:
  - csharp
---

# Migrating from MvvmLight

This article outlines some of the key differences between the [MvvmLight Toolkit](https://github.com/lbugnion/mvvmlight) and the Windows Community Toolkit MVVM framework to ease your migration. 

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

#### VerifyPropertyName ( string )

There is no direct replacement for the `VerifyPropertyName(string)` method and any code using this should be altered or removed.

```csharp
// MvvmLight
this.VerifyPropertyName(nameof(this.MyProperty));

// Toolkit.Mvvm
// No direct replacement, remove
```

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

#### ICleanup.Cleanup ()

There is no direct replacement for the `ICleanup` interface.

However, the `ObservableRecipient` provides an `OnDeactivated` method which should be used to provide the same functionality as `Cleanup`. 

```csharp
// MvvmLight
this.Cleanup();

// Toolkit.Mvvm
this.OnDeactivated();
```

Note, the `OnActivated` and `OnDeactivated` methods can be called from your existing solution as with `Cleanup`. 

However, the `ObservableRecipient` exposes an `IsActive` property that also controls the call to these methods when it is set. 

#### Set<T> ( string, ref T, T, bool )

`Set<T>(string, ref T, T, bool)` does not have a like-for-like method signature replacement. 

However, `SetProperty<T>(ref T, T, bool, string)` provides the same functionality with re-ordered parameters.

```csharp
// MvvmLight
this.Set(nameof(this.MyProperty), ref this.myProperty, value, true);

// Toolkit.Mvvm
this.SetProperty(ref this.myProperty, value, true, nameof(this.MyProperty));
```

Note, the value and broadcast boolean parameters are not optional in the Toolkit's implementation and must be provided to use this method.

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

There is no direct replacement for the `RaisePropertyChanged<T>(string, T, T, bool)` method and any code using this should be altered or removed.

```csharp
// MvvmLight
this.RaisePropertyChanged<MyObject>(nameof(this.MyProperty), this.oldValue, this.newValue, true);

// Toolkit.Mvvm
// No direct replacement, remove
```

#### RaisePropertyChanged<T> ( Expression, T, T, bool )

There is no direct replacement for the `RaisePropertyChanged<T>(Expression, T, T, bool)` method.

It is recommended for improved performance that you replace this with the Toolkit's `RaisePropertyChanged<T>(string, T, T bool)` using the `nameof` keyword instead.

```csharp
// MvvmLight
this.RaisePropertyChanged<MyObject>(() => this.MyProperty, this.oldValue, this.newValue, true);

// Toolkit.Mvvm
this.RaisePropertyChanged<MyObject>(nameof(this.MyProperty), this.oldValue, this.newValue, true);
```

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

```csharp
// MvvmLight
var isInDesignMode = this.IsInDesignMode;

// Toolkit.Mvvm
// No direct replacement, remove
```

### ViewModelBase Static Properties

#### IsInDesignModeStatic

There is no direct replacement for the `IsInDesignModeStatic` property and any code using this should be altered or removed.

```csharp
// MvvmLight
var isInDesignMode = ViewModelBase.IsInDesignModeStatic;

// Toolkit.Mvvm
// No direct replacement, remove
```

## SimpleIoc

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