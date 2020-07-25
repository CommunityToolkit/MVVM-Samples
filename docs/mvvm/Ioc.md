---
title: Ioc
author: Sergio0694
description: A type that facilitates the use of the IServiceProvider type
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, service, dependency injection, net core, net standard
dev_langs:
  - csharp
---

# Ioc

The [`Ioc`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.DependencyInjection.Ioc) class is a type that facilitates the use of the `IServiceProvider` type. It's powered by the `Microsoft.Extensions.DependencyInjection` package, which provides a fully featured and powerful DI set of APIs, and acts as an easy to setup and use `IServiceProvider`.

## Configure and resolve services

The main entry point is the `ConfigureServices` method, which can be used like so:

```csharp
// Register the services at startup
Ioc.Default.ConfigureServices(services =>
{
    services.AddSingleton<IFilesService, FilesService>();
    services.AddSingleton<ISettingsService, SettingsService>();
    // Other services here...
});

// Retrieve a service instance when needed
IFilesService fileService = Ioc.Default.GetService<IFilesService>();
```

The `Ioc.Default` property offers a thread-safe `IServiceProvider` instance that can be used anywhere in the application to resolve services. The `ConfigureService` method handles the initialization of that service. It is also possible to create different `Ioc` instances and to initialize each with different services.

## Constructor injection

One powerful feature that is available is "constructor injection", which means that the DI service provider is able to automatically resolve indirect dependencies between registered services when creating instances of the type being requested. Consider the following service:

```csharp
public class ConsoleLogger : ILogger
{
    private readonly IFileService FileService;
    private readonly IConsoleService ConsoleService;

    public FileLogger(
        IFileService fileService,
        IConsoleService consoleService)
    {
        FileService = fileService;
        ConsoleService = consoleService;
    }

    // Methods for the IFileLogger interface here...
}
```

Here we have a `ConsoleLogger` implementing the `ILogger` interface, and requiring `IFileService` and `IConsoleService` instances. Constructor injection means the DI service provider will "automagically" gather all the necessary services, like so:

```csharp
// Register the services at startup
Ioc.Default.ConfigureServices(services =>
{
    services.AddSingleton<IFileService, FileService>();
    services.AddSingleton<IConsoleService, ConsoleService>();
    services.AddSingleton<ILogger, ConsoleLogger>();
});

// Retrieve a console logger with constructor injection
ConsoleLogger consoleLogger = Ioc.Default.GetService<ConsoleLogger>();
```

The DI service provider will automatically check whether all the necessary services are registered, then it will retrieve them and invoke the constructor for the registered `ILogger` concrete type, to get the instance to return - all done automatically!

## More docs

For more info about `Microsoft.Extensions.DependencyInjection`, see [here](https://docs.microsoft.com/aspnet/core/fundamentals/dependency-injection).

## Sample Code

You can find more examples in our [unit tests](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/UnitTests/UnitTests.Shared/Mvvm)

## Requirements

| Device family | Universal, 10.0.16299.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Mvvm |
| NuGet package | [Microsoft.Toolkit.Mvvm](https://www.nuget.org/packages/Microsoft.Toolkit.Mvvm/) |

## API

* [ObservableObject source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Mvvm/ComponentModel/ObservableObject.cs)
