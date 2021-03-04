---
title: RelayCommand
author: Sergio0694
description: A command whose sole purpose is to relay its functionality to other objects by invoking delegates
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, componentmodel, property changed, notification, binding, command, delegate, net core, net standard
dev_langs:
  - csharp
---

# RelayCommand and RelayCommand&lt;T>

The [`RelayCommand`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.input.RelayCommand) and [`RelayCommand<T>`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.input.RelayCommand-1) are `ICommand` implementations that can expose a method or delegate to the view. These types act as a way to bind commands between the viewmodel and UI elements.

> **Platform APIs:** [`RelayCommand`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.input.RelayCommand), [`RelayCommand<T>`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.input.RelayCommand-1), [`IRelayCommand`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.input.IRelayCommand), [`IRelayCommand<T>`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.input.IRelayCommand-1)

## How they work

`RelayCommand` and `RelayCommand<T>` have the following main features:

- They provide a base implementation of the `ICommand` interface.
- They also implement the [`IRelayCommand`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.input.IRelayCommand) (and [`IRelayCommand<T>`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.input.IRelayCommand-1)) interface, which exposes a `NotifyCanExecuteChanged` method to raise the `CanExecuteChanged` event.
- They expose constructors taking delegates like `Action` and `Func<T>`, which allow the wrapping of standard methods and lambda expressions.

## Working with `ICommand`

The following shows how to set up a simple command:

```csharp
public class MyViewModel : ObservableObject
{
    public MyViewModel()
    {
        IncrementCounterCommand = new RelayCommand(IncrementCounter);
    }

    private int counter;

    public int Counter
    {
        get => counter;
        private set => SetProperty(ref counter, value);
    }

    public ICommand IncrementCounterCommand { get; }

    private void IncrementCounter() => Counter++;
}
```

And the relative UI could then be (using WinUI XAML):

```xml
<Page
    x:Class="MyApp.Views.MyPage"
    xmlns:viewModels="using:MyApp.ViewModels">
    <Page.DataContext>
        <viewModels:MyViewModel x:Name="ViewModel"/>
    </Page.DataContext>

    <StackPanel Spacing="8">
        <TextBlock Text="{x:Bind ViewModel.Counter, Mode=OneWay}"/>
        <Button
            Content="Click me!"
            Command="{x:Bind ViewModel.IncrementCounterCommand}"/>
    </StackPanel>
</Page>
```

The `Button` binds to the `ICommand` in the viewmodel, which wraps the private `IncrementCounter` method. The `TextBlock` displays the value of the `Counter` property and is updated every time the property value changes.

## Examples

You can find more examples in the [unit tests](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/UnitTests/UnitTests.Shared/Mvvm).