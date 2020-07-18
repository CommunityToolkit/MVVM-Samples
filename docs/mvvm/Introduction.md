---
title: Introduction to the MVVM package
author: Sergio0694
description: An overview of how to get started with MVVM package and to the APIs it contains
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, get started, visual studio, MVVM, net core, net standard
---

# Introduction to the MVVM package

The `Microsoft.Toolkit.Mvvm` package is a modern, fast and modular MVVM library, part of the Windows Community Toolkit. It's built around the following key principles:

- **Platform and Runtime Independent** - **.NET Standard 2.0** 🚀 (UI Framework Agnostic)
- **Simple to pick-up and use** - No strict requirements on Application structure or coding-paradigms (outside of 'MVVM'ness) i.e. flexible usage
- **À la carte** - Developer able to choose the components they wish to leverage
- **Reference Implementation** - Lean and performant, provides compliments to interfaces and paradigms hinted at in the Base-Class Library, but without provided implementations.

The fact that this package only targets .NET Standard means that it can be used on any app platform, from UWP to WinForms or WPF, to Xamarin or Uno, and more, and on any runtime from .NET Native to .NET Core, .NET Framework or Mono. The API surface is identical in all cases, which makes this library perfect to build shared backend libraries for applications.

Follow these steps to install the MVVM package:

1. Open an existing project in Visual studio, targeting any of the following:
    - UWP (>= 10.0)
    - .NET Standard (>= 1.4)
    - .NET Core (>= 1.0)
    - Any other framework supporting .NET Standard 2.0 and up

2. In Solution Explorer panel, right click on your project name and select **Manage NuGet Packages**. Search for **Microsoft.Toolkit.Mvvm** and install it.

    ![NuGet Packages](../resources/images/ManageNugetPackages.png "Manage NuGet Packages Image")

3. Add a using directive in your C# files to use the new APIs:

    ```c#
    using Microsoft.Toolkit.Mvvm;
    ```

4. If you want so see some code samples, you can either read through the other docs pages for the MVVM package, or have a look at the various [unit tests](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/master/UnitTests/UnitTests.Shared/Mvvm) for the project.

## When should I use this package?

The idea for this package would be to have a series of common, self-contained, lightweight types:

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

These types can be used as a base to build modern apps using the MVVM pattern, as they provide a starting implementation of all the main primitives that are necessary (property change notifications, commands, dependency injection, etc.). These types alone are usually enough for many users building apps using the MVVM toolkit, without the need to add additional external references.

One key point of this package is also to offer as much flexibility as possible: developers are free to pick and choose which components to use, and all the available types are loosely coupled so that it's possible to just pick as many of them as needed and only use them in applications. There is no requirement to go "all-in" with a specific series of all encompassing APIs, nor is there a set of mandatory patterns that need to be followed when building apps using these helpers. It's really up to each developer to combine these building blocks in the way that best fit their needs.
