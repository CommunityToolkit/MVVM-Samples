---
title: RelayCommand
author: Sergio0694
description: A command whose sole purpose is to relay its functionality to other objects by invoking delegates
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, componentmodel, property changed, notification, binding, command, delegate, net core, net standard
dev_langs:
  - csharp
---

# RelayCommand and RelayCommand&lt;T>

The [`RelayCommand`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.input.RelayCommand) and [`RelayCommand<T>`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.input.RelayCommand-1) are two `ICommand` implementations that can be used to expose a specified method or delegate to the view. These types act as a way to bind commands from the viewmodel to UI elements.

## How they work

`RelayCommand` and `RelayCommand<T>` have the following main features:

- They provide a base implementation for the `ICommand` interface.
- They also implement the `IRelayCommand` (and `IRelayCommand<T>`) interface, which exposes a `NotifyCanExecuteChanged` method that can be used to externally raise the `CanExecuteChanged` event when needed.
- They expose constructors taking delegates like `Action` and `Func<T>`, which allow to wrap either standard methods or lambda expressions.

## Working with `ICommand`

Here's an example of how to setup a simple command:

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
        private set => Set(ref counter, value);
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

We can see how the `Button` binds to the `ICommand` we have in the viewmodel, which again wraps the private `IncrementCounter` method. The `TextBlock` simply displays the value of the `Counter` property, updating its representation every time the property value changes.

## Sample Code

You can find more examples in our [unit tests](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/UnitTests/UnitTests.Shared/Mvvm)

## Requirements

| Device family | Universal, 10.0.16299.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Mvvm |
| NuGet package | [Microsoft.Toolkit.Mvvm](https://www.nuget.org/packages/Microsoft.Toolkit.Mvvm/) |

## API

* [RelayCommand source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Mvvm/Input/RelayCommand.cs)
* [RelayCommand&lt;T> source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Mvvm/Input/RelayCommand{T}.cs)
