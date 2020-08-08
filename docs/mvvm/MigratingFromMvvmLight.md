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

#### RaisePropertyChanged ( Expression )

`RaisePropertyChanged(Expression)` does not have a direct replacement. 

It is recommended for improved performance that you replace `RaisePropertyChanged(Expression)` with the Toolkit's `OnPropertyChanged(string)` using the `nameof` keyword instead.

```csharp
// MvvmLight
this.RaisePropertyChanged(() => this.MyProperty);

// Toolkit.Mvvm
this.OnPropertyChanged(nameof(this.MyProperty));
```

#### Set ( Expression, ref, value )

`Set(Expression, ref, value)` does not have a like-for-like method signature replacement. 

However, `SetProperty(Expression, value)` provides the same functionality without the additional reference parameter.

```csharp
// MvvmLight
this.Set(() => this.MyProperty, ref this.myProperty, value);

// Toolkit.Mvvm
this.SetProperty(() => this.MyProperty, value);
```

#### Set ( string, ref, value )

`Set(string, ref, value)` does not have a like-for-like method signature replacement. 

However, `SetProperty(ref, value, string)` provides the same functionality with re-ordered parameters.

```csharp
// MvvmLight
this.Set(nameof(this.MyProperty), ref this.myProperty, value);

// Toolkit.Mvvm
this.SetProperty(ref this.myProperty, value, nameof(this.MyProperty));
```

#### Set ( ref, value, string )

`Set(ref, value, string)` has a renamed direct replacement, `SetProperty(ref, value, string)`.

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