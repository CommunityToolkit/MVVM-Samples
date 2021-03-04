---
title: Ioc
author: Sergio0694
description: An introduction to the use of the IServiceProvider type through the Microsoft.Extensions.DependencyInjection APIs
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, service, dependency injection, net core, net standard
dev_langs:
  - csharp
---

# Ioc ([Inversion of control](https://en.wikipedia.org/wiki/Inversion_of_control))

A common pattern that can be used to increase modularity in the codebase of an application using the MVVM pattern is to use some form of inversion of control. One of the most common solution in particular is to use dependency injection, which consists in creating a number of services that are injected into backend classes (ie. passed as parameters to the viewmodel constructors) - this allows code using these services not to rely on implementation details of these services, and it also makes it easy to swap the concrete implementations of these services. This pattern also makes it easy to make platform-specific features available to backend code, by abstracting them through a service which is then injected where needed.

The MVVM Toolkit doesn't provide built-in APIs to facilitate the usage of this pattern, as there already exist dedicated libraries specifically for this such as the `Microsoft.Extensions.DependencyInjection` package, which provides a fully featured and powerful DI set of APIs, and acts as an easy to setup and use `IServiceProvider`. The following guide will refer to this library and provide a series of examples of how to integrate it into applications using the MVVM pattern.

> **Platform APIs:** [`Ioc`](Microsoft.Toolkit.Mvvm.DependencyInjection.Ioc)

## Configure and resolve services

The first step is to declare an `IServiceProvider` instance, and to initialize all the necessary services, usually at startup. For instance, on UWP (but a similar setup can be used on other frameworks too):

```csharp
public sealed partial class App : Application
{
    public App()
    {
        Services = ConfigureServices();

        this.InitializeComponent();
    }

    /// <summary>
    /// Gets the current <see cref="App"/> instance in use
    /// </summary>
    public new static App Current => (App)Application.Current;

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IFilesService, FilesService>();
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<IClipboardService, ClipboardService>();
        services.AddSingleton<IShareService, ShareService>();
        services.AddSingleton<IEmailService, EmailService>();

        return services.BuildServiceProvider();
    }
}
```

Here the `Services` property is initialized at startup, and all the application services and viewmodels are registered. There is also a new `Current` property that can be used to easily access the `Services` property from other views in the application. For instance:

```csharp
IFilesService filesService = App.Current.Services.GetService<IFilesService>();

// Use the files service here...
```

The key aspect here is that each service may very well be using platform-specific APIs, but since those are all abstracted away through the interface our code is using, we don't need to worry about them whenever we're just resolving an instance and using it to perform operations.

## Constructor injection

One powerful feature that is available is "constructor injection", which means that the DI service provider is able to automatically resolve indirect dependencies between registered services when creating instances of the type being requested. Consider the following service:

```csharp
public class FileLogger : IFileLogger
{
    private readonly IFilesService FileService;
    private readonly IConsoleService ConsoleService;

    public FileLogger(
        IFilesService fileService,
        IConsoleService consoleService)
    {
        FileService = fileService;
        ConsoleService = consoleService;
    }

    // Methods for the IFileLogger interface here...
}
```

Here we have a `FileLogger` type implementing the `IFileLogger` interface, and requiring `IFilesService` and `IConsoleService` instances. Constructor injection means the DI service provider will automatically gather all the necessary services, like so:

```csharp
/// <summary>
/// Configures the services for the application.
/// </summary>
private static IServiceProvider ConfigureServices()
{
    var services = new ServiceCollection();

    services.AddSingleton<IFilesService, FilesService>();
    services.AddSingleton<IConsoleService, ConsoleService>();
    services.AddSingleton<IFileLogger, FileLogger>();

    return services.BuildServiceProvider();
}

// Retrieve a logger service with constructor injection
IFileLogger fileLogger = App.Current.Services.GetService<IFileLogger>();
```

The DI service provider will automatically check whether all the necessary services are registered, then it will retrieve them and invoke the constructor for the registered `IFileLogger` concrete type, to get the instance to return.

## What about viewmodels?

A service provider has "service" in its name, but it can actually be used to resolve instances of any class, including viewmodels! The same concepts explained above still apply, including constructor injection. Imagine we had a `ContactsViewModel` type, using an `IContactsService` and an `IPhoneService` instance through its constructor. We could have a `ConfigureServices` method like this:

```csharp
/// <summary>
/// Configures the services for the application.
/// </summary>
private static IServiceProvider ConfigureServices()
{
    var services = new ServiceCollection();

    // Services
    services.AddSingleton<IContactsService, ContactsService>();
    services.AddSingleton<IPhoneService, PhoneService>();

    // Viewmodels
    services.AddTransient<ContactsViewModel>();

    return services.BuildServiceProvider();
}
```

And then in our `ContactsView`, we would assign the data context as follows:

```csharp
public ContactsView()
{
    this.InitializeComponent();
    this.DataContext = App.Current.Services.GetService<ContactsViewModel>();
}
```

## More docs

For more info about `Microsoft.Extensions.DependencyInjection`, see [here](https://docs.microsoft.com/aspnet/core/fundamentals/dependency-injection).

## Examples

You can find more examples in the [unit tests](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/UnitTests/UnitTests.Shared/Mvvm).