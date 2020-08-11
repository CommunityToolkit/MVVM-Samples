---
title: AsyncRelayCommand
author: Sergio0694
description: An asynchronous command whose sole purpose is to relay its functionality to other objects by invoking delegates
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, componentmodel, property changed, notification, binding, command, delegate, net core, net standard
dev_langs:
  - csharp
---

# AsyncRelayCommand and AsyncRelayCommand&lt;T>

The [AsyncRelayCommand](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.input.AsyncRelayCommand) and [AsyncRelayCommand<T>](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.input.AsyncRelayCommand-1) are `ICommand` implementations that extend the functionalities offered by `RelayCommand`, with support for asynchronous operations.

## How they work

`AsyncRelayCommand` and `AsyncRelayCommand<T>` have the following main features:

- They extend the functionalities of the non-asynchronous commands included in the library, with support for `Task`-returning delegates.
- They expose an `ExecutionTask` property that can be used to monitor the progress of a pending operation, and an `IsRunning` that can be used to check when an operation completes. This is particularly useful to bind a command to UI elements such as loading indicators.
- They implement the `IAsyncRelayCommand` and `IAsyncRelayCommand<T>` interfaces, which means that viewmodel can easily expose commands using these to reduce the tight coupling between types. For instance, this makes it easier to replace a command with a custom implementation exposing the same public API surface, if needed.

## Working with asynchronous commands

Let's imagine a scenario similar to the one described in the `RelayCommand` sample, but a command executing an asynchronous operation.

Upon clicking the `Button`, the command is invoked, and the `ExecutionTask` updated. When the operation completes, the property raises a notification which is reflected in the UI. In this case, both the task status and the current result of the task are displayed. Note that to show the result of the task, it is necessary to use the `TaskExtensions.GetResultOrDefault` method - this provides access to the result of a task that has not yet completed without blocking the thread (and possibly causing a deadlock).

## Sample Code

There are more examples in the [unit tests](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/UnitTests/UnitTests.Shared/Mvvm).

## Requirements

| Device family | Universal, 10.0.16299.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Mvvm |
| NuGet package | [Microsoft.Toolkit.Mvvm](https://www.nuget.org/packages/Microsoft.Toolkit.Mvvm/) |

## API

* [AsyncRelayCommand source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Mvvm/Input/AsyncRelayCommand.cs)
* [AsyncRelayCommand&lt;T> source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Mvvm/Input/AsyncRelayCommand{T}.cs)
