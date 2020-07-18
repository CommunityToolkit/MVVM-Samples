---
title: Introduction to the MVVM package
author: Sergio0694
description: An overview of how to get started with MVVM package and to the APIs it contains
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, get started, visual studio, MVVM, net core, net standard
dev_langs:
  - csharp
  - vb 
---


# Introduction to the MVVM package

The `Microsoft.Toolkit.Mvvm` package is a modern, fast, and modular MVVM library. It is part of the Windows Community Toolkit. It follows these key principles:

- **Platform and Runtime Independent** - **.NET Standard 2.0** 🚀 (UI Framework Agnostic)
- **Simple to pick-up and use** - No strict requirements on Application structure or coding-paradigms (outside of 'MVVM'ness) i.e. flexible usage
- **À la carte** - Developer able to choose the components they wish to leverage
- **Reference Implementation** - Lean and performant, provides compliments to interfaces and paradigms hinted at in the Base Class Library, but without provided implementations.

This package only targets .NET Standard so it can be used on any app platform, from UWP to WinForms or WPF, to Xamarin or Uno, and more, and on any runtime from .NET Native to .NET Core, .NET Framework or Mono. The API surface is identical in all cases, making this library perfect to build shared libraries whether you're using:
 - UWP (>= 10.0)
- .NET Standard (>= 1.4)
- .NET Core (>= 1.0)
 - Any other framework supporting .NET Standard 2.0 and up

To install the package from within Visual Studio:

1. In Solution Explorer panel, right-click on your project, and select **Manage NuGet Packages**. Search for **Microsoft.Toolkit.Mvvm** and install it.

    ![NuGet Packages](../resources/images/ManageNugetPackages.png "Manage NuGet Packages Image")

2. Add a using or imports directive in your files to use the new APIs:

    ```csharp
    using Microsoft.Toolkit.Mvvm;
    ```
    ```vb
    Imports Microsoft.Toolkit.Mvvm
    ```

3. Code samples, are available in the other docs pages for the MVVM package, and in the [unit tests](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/master/UnitTests/UnitTests.Shared/Mvvm) for the project.

## When should I use this package?

This package provides a series of common, self-contained, lightweight types:

- **Microsoft.Toolkit.Mvvm.ComponentModel**
  - `ObservableObject`
  - `ObservableRecipient`
- **Microsoft.Toolkit.Mvvm.DependencyInjection**
  - `Ioc`
- **Microsoft.Toolkit.Mvvm.Input**
  - `RelayCommand`
  - `RelayCommand<T>`
  - `AsyncRelayCommand`
  - `AsyncRelayCommand<T>`
  - `IRelayCommand`
  - `IRelayCommand<in T>`
  - `IAsyncRelayCommand`
  - `IAsyncRelayCommand<in T>`
- **Microsoft.Toolkit.Mvvm.Messaging**
  - `Messenger`
  - `IMessenger`
- **Microsoft.Toolkit.Mvvm.Messaging.Messages**
  - `PropertyChangedMessage<T>`
  - `RequestMessage<T>`
  - `AsyncRequestMessage<T>`
  - `CollectionRequestMessage<T>`
  - `AsyncCollectionRequestMessage<T>`
  - `ValueChangedMessage<T>`

These types provide a starting implementation of all the main primitives necessary to build modern apps using the MVVM pattern. These types alone are usually enough for many users to build apps using the MVVM toolkit, without needing additional external references.

This package aims to offer as much flexibility as possible, so developers are free to choose which components to use.  All types are loosely-coupled, so that it's only necessary to include what you use. There is no requirement to go "all-in" with a specific series of all-encompassing APIs, nor is there a set of mandatory patterns that need to be followed when building apps using these helpers. Developers are free to combine these building blocks in the way that best fit their needs.
