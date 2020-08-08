---
title: Migrating from MvvmLight
author: jamesmcroft
description: This article describes how to migrate MVVM Light solutions to the Windows Community Toolkit MVVM framework.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, mvvmlight, net core, net standard
dev_langs:
  - csharp
---

# Migrating from MvvmLight

This article outlines some of the key differences between the [MVVM Light Toolkit](https://github.com/lbugnion/mvvmlight) and the Windows Community Toolkit MVVM framework to ease your migration. 

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

The following steps focus on migrating your existing components which take advantage of the `ObservableObject` of the MVVM Light Toolkit. 

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

#### Set<T> ( string, ref T, T )

`Set<T>(string, ref T, T)` does not have a like-for-like method signature replacement. 

However, `SetProperty<T>(ref T, T, string)` provides the same functionality with re-ordered parameters.

```csharp
// MvvmLight
this.Set(nameof(this.MyProperty), ref this.myProperty, value);

// Toolkit.Mvvm
this.SetProperty(ref this.myProperty, value, nameof(this.MyProperty));
```

#### Set<T> ( ref T, T, string )

`Set<T>(ref T, T, string)` has a renamed direct replacement, `SetProperty<T>(ref T, T, string)`.

```csharp
// MvvmLight
this.Set(ref this.myProperty, value, nameof(this.MyProperty));

// Toolkit.Mvvm
this.SetProperty(ref this.myProperty, value, nameof(this.MyProperty));
```

### ObservableObject Properties

#### PropertyChangedHandler

`PropertyChangedHandler` does not have a direct replacement. 

However, the same implementation can be achieved by accessing the `PropertyChanged` event handler from the `ObservableObject`.

```csharp
// MvvmLight
PropertyChangedEventHandler handler = this.PropertyChangedHandler;

// Toolkit.Mvvm
PropertyChangedEventHandler handler = this.PropertyChanged;
```

## Migrating ViewModelBase

The following steps focus on migrating your existing components which take advantage of the `ViewModelBase` of the MVVM Light Toolkit. 

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

#### Set<T> ( ref T, T, bool, string )

`Set<T>(ref T, T, bool, string)` has a renamed direct replacement, `SetProperty<T>(ref T, T, bool, string)`. 

```csharp
// MvvmLight
this.Set(ref this.myProperty, value, true, nameof(this.MyProperty));

// Toolkit.Mvvm
this.SetProperty(ref this.myProperty, value, true, nameof(this.MyProperty));
```

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

#### RaisePropertyChanged<T> ( string, T, T, bool )

There is no direct replacement for the `RaisePropertyChanged<T>(string, T, T, bool)` method and any code using this should be altered or removed.

```csharp
// MvvmLight
this.RaisePropertyChanged<MyObject>(nameof(this.MyProperty), this.oldValue, this.newValue, true);

// Toolkit.Mvvm
// No direct replacement, remove
```

#### RaisePropertyChanged<T> ( Expression, T, T, bool )

There is no direct replacement for the `RaisePropertyChanged<T>(Expression, T, T, bool)` method and any code using this should be altered or removed.

```csharp
// MvvmLight
this.RaisePropertyChanged<MyObject>(() => this.MyProperty, this.oldValue, this.newValue, true);

// Toolkit.Mvvm
// No direct replacement, remove
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