---
title: Introduction to the MVVM Toolkit
author: Sergio0694
description: An overview of how to get started with the MVVM Toolkit and to the APIs it contains
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, get started, visual studio, MVVM, net core, net standard
dev_langs:
  - csharp
  - vb 
---

# Introduction to the MVVM Toolkit

The `Microsoft.Toolkit.Mvvm` package (aka MVVM Toolkit) is a modern, fast, and modular MVVM library. It is part of the Windows Community Toolkit and is built around the following principles:

- **Platform and Runtime Independent** - **.NET Standard 2.0** and **.NET 5** 🚀 (UI Framework Agnostic)
- **Simple to pick-up and use** - No strict requirements on Application structure or coding-paradigms (outside of 'MVVM'ness), i.e., flexible usage.
- **À la carte** - Freedom to choose which components to use.
- **Reference Implementation** - Lean and performant, providing implementations for interfaces that are included in the Base Class Library, but lack concrete types to use them directly.

This package targets **.NET Standard** so it can be used on any app platform: UWP, WinForms, WPF, Xamarin, Uno, and more; and on any runtime: .NET Native, .NET Core, .NET Framework, or Mono. It runs on all of them. The API surface is identical in all cases, making it perfect for building shared libraries.

Additionally, the MVVM Toolkit also has a **.NET 5** target, which is used to enable more internal optimizations when running in on the .NET 5 runtime. The public API surface is identical in both cases, so NuGet will always resolve the best possible version of the package without consumers having to worry about which APIs will be available on their platform.

To install the package from within Visual Studio:

1. In Solution Explorer, right-click on the project and select **Manage NuGet Packages**. Search for **Microsoft.Toolkit.Mvvm** and install it.

    ![NuGet Packages](../resources/images/ManageNugetPackages.png "Manage NuGet Packages Image")

2. Add a using or Imports directive to use the new APIs:

    ```c#
    using Microsoft.Toolkit.Mvvm;
    ```

    ```vb
    Imports Microsoft.Toolkit.Mvvm
    ```

3. Code samples are available in the other docs pages for the MVVM Toolkit, and in the [unit tests](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/rel/7.1.0/UnitTests/UnitTests.Shared/Mvvm) for the project.

## When should I use this package?

Use this package for access to a collection of standard, self-contained, lightweight types that provide a starting implementation for building modern apps using the MVVM pattern. These types alone are usually enough for many users to build apps without needing additional external references.

The included types are:

- **Microsoft.Toolkit.Mvvm.ComponentModel**
  - [`ObservableObject`](ObservableObject.md)
  - [`ObservableRecipient`](ObservableRecipient.md)
  - [`ObservableValidator`](ObservableValidator.md)
- **Microsoft.Toolkit.Mvvm.DependencyInjection**
  - [`Ioc`](Ioc.md)
- **Microsoft.Toolkit.Mvvm.Input**
  - [`RelayCommand`](RelayCommand.md)
  - [`RelayCommand<T>`](RelayCommand.md)
  - [`AsyncRelayCommand`](AsyncRelayCommand.md)
  - [`AsyncRelayCommand<T>`](AsyncRelayCommand.md)
  - [`IRelayCommand`](RelayCommand.md)
  - [`IRelayCommand<in T>`](RelayCommand.md)
  - [`IAsyncRelayCommand`](AsyncRelayCommand.md)
  - [`IAsyncRelayCommand<in T>`](AsyncRelayCommand.md)
- **Microsoft.Toolkit.Mvvm.Messaging**
  - [`IMessenger`](Messenger.md)
  - [`WeakReferenceMessenger`](Messenger.md)
  - [`StrongReferenceMessenger`](Messenger.md)
  - [`IRecipient<TMessage>`](Messenger.md)
  - [`MessageHandler<TRecipient, TMessage>`](Messenger.md)
- **Microsoft.Toolkit.Mvvm.Messaging.Messages**
  - [`PropertyChangedMessage<T>`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.Messages.PropertyChangedMessage-1)
  - [`RequestMessage<T>`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.Messages.RequestMessage-1)
  - [`AsyncRequestMessage<T>`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.Messages.AsyncRequestMessage-1)
  - [`CollectionRequestMessage<T>`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.Messages.CollectionRequestMessage-1)
  - [`AsyncCollectionRequestMessage<T>`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.Messages.AsyncCollectionRequestMessage-1)
  - [`ValueChangedMessage<T>`](/dotnet/api/microsoft.toolkit.mvvm.Messaging.Messages.ValueChangedMessage-1)

This package aims to offer as much flexibility as possible, so developers are free to choose which components to use.  All types are loosely-coupled, so that it's only necessary to include what you use. There is no requirement to go "all-in" with a specific series of all-encompassing APIs, nor is there a set of mandatory patterns that need to be followed when building apps using these helpers. Combine these building blocks in a way that best fits your needs.

## Additional resources

- Check out the [sample app](https://github.com/windows-toolkit/MVVM-Samples) (for multiple UI frameworks) to see the MVVM Toolkit in action.
- You can also find more examples in the [unit tests](https://github.com/windows-toolkit/WindowsCommunityToolkit/blob/rel/7.1.0/UnitTests/UnitTests.Shared/Mvvm).
