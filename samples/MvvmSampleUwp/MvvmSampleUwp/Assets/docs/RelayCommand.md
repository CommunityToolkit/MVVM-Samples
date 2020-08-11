---
title: RelayCommand
author: Sergio0694
description: A command whose sole purpose is to relay its functionality to other objects by invoking delegates
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, componentmodel, property changed, notification, binding, command, delegate, net core, net standard
dev_langs:
  - csharp
---

# RelayCommand and RelayCommand&lt;T>

The [RelayCommand](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.input.RelayCommand) and [RelayCommand<T>](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.input.RelayCommand-1) are `ICommand` implementations that can expose a method or delegate to the view. These types act as a way to bind commands between the viewmodel and UI elements.

## How they work

`RelayCommand` and `RelayCommand<T>` have the following main features:

- They provide a base implementation of the `ICommand` interface.
- They also implement the `IRelayCommand` (and `IRelayCommand<T>`) interface, which exposes a `NotifyCanExecuteChanged` method to raise the `CanExecuteChanged` event.
- They expose constructors taking delegates like `Action` and `Func<T>`, which allow the wrapping of standard methods and lambda expressions.

## Working with `ICommand`

The following sample shows how to set up a simple command using `RelayCommand` to abstract a method in the viewmodel. We also have a property raising change notifications through the `SetProperty` method inherited from `ObservableObject`. The UI has a `Button` control binding to the `ICommand` in the viewmodel, and a `TextBlock` that displays the value of the `Counter` property.

## Sample Code

There are more examples in the [unit tests](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/UnitTests/UnitTests.Shared/Mvvm).

## Requirements

| Device family | Universal, 10.0.16299.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Mvvm |
| NuGet package | [Microsoft.Toolkit.Mvvm](https://www.nuget.org/packages/Microsoft.Toolkit.Mvvm/) |

## API

* [RelayCommand source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Mvvm/Input/RelayCommand.cs)
* [RelayCommand&lt;T> source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Mvvm/Input/RelayCommand{T}.cs)
