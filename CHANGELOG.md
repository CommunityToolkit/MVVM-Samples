# API changes in Preview 4:

🆕 Reintroduced `Ioc` class

🆕 Added new `ObservableObject.OnPropertyChanged(PropertyChangedEventArgs)` overload

🆕 Added new `ObservableObject.OnPropertyChanging(PropertyChangingEventArgs)` overload

🆕 Added new `TrySetProperty` methods to `ObservableValidator`

✅ The `OnPropertyChanged(string)` and `OnPropertyChanging(string)` overloads with a string param are no longer virtual (💥)

✅ Added notification support to `IAsyncRelayCommand.CanBeCanceled` property

✅ Improved notification logic for other `IAsyncRelayCommand` properties

✅ Added notification support to `ObservableValidator.HasErrors` property

✅ Minor performance/memory usage improvements and bug fixes

## Breaking changes

💥 If you were overriding `OnPropertyChanged(string)` or `OnPropertyChanging(string)`, you should now override the overloads taking `PropertyChangedEventArgs` and `PropertyChangingEventArgs` instead, and move your additional logic there.

## A note on the `Ioc` class

The `Ioc` class exposes similar APIs to the ones in Preview 2, but it now doesn't include a direct reference to `Microsoft.Extensions.DependencyInjection`. You can add a reference to any DI library you wish to use, and then you can use the `Ioc.Default.ConfigureServices(IServiceProvider)` method to initialize the `Ioc.Default` instance. From there, you'll be able to use it normally just like in Preview 2. For instance:

```cs
// Preview 2
Ioc.Default.ConfigureServices(services =>
{
    services.AddSingleton<ILogger, Logger>();
    services.AddSingleton<IDialogService, DialogService>();
    // Other services...
});

// Preview 3
Ioc.Default.ConfigureServices(
    new ServiceCollection()
    .AddSingleton<ILogger, Logger>()
    .AddSingleton<IDialogService, DialogService>()
    .BuildServiceProvider());
```

Here we're using the `ServiceCollection` class from the `Microsoft.Extensions.DependencyInjection` library, but you're free to use any other DI library as well - the `Ioc` class only needs an input `IServiceProvider` instance to work.

# API changes in Preview 3:

🆕 New `ObservableValidator` class, which supports the [`INotifyDataErrorInfo`](https://docs.microsoft.com/dotnet/api/system.componentmodel.inotifydataerrorinfo) interface.

🆕 Added new constructors with cancellation support to the async commands, and added new cancellation-related properties to the async command interfaces.

🆕 Added a new `WeakReferenceMessenger` type. This type is less performant than the other messenger, and uses more memory, and in return only uses weak references to track recipients. This type essentially mirrors the behavior of the `Messenger` type from `MvvmLight`, making the transition easier for developers migrating from that library.

🆕 Introduced a new custom delegate to represent message handlers, which also receives the current recipient as additional input parameter (💥 breaking change, see code changes below).

✅ Renamed `Messenger` to `StrongReferenceMessenger` (💥).

✅ The `WeakReferenceMessenger` is now the default messenger used by the `ObservableRecipient` class (💥).

✅ Changed `ObservableObject` overloads using `Expression<Func<T>>` to be more efficient (💥).

✅ API changes to the `SetPropertyAndNotifyOnCompletion` (as detailed in [this blog post]( https://devblogs.microsoft.com/pax-windows/mvvm-toolkit-preview-3-the-journey-of-an-api/), 💥).

🚨 Removed the `Ioc` class (we will include docs on how to easily start using the `Microsoft.Extensions.DependencyInjection` library directly to work with dependency injection, 💥).

## Breaking changes

💥 If you were registering message handlers, no API changes are required for messages registered through the `IRecipient<TMessage>` interface. If you were manually registering handlers with the `Action<TMessage>` delegate instead, you will need to modify their code as follows:

```cs
// Preview 2
Messenger.Register<MyMessage>(this, message =>
{
    // Do stuff with the message here...
    // Note that invoking this instance method means that
    // the lambda expression is also capturing "this".
    // This issue was also present in MvvmLight.
    SomeInstanceMethod();
});

// Preview 3
Messenger.Register<MyViewModel, MyMessage>(this, (recipient, message) =>
{
    // Do stuff here...
    // Note that since we're accessing the recipient from the
    // input parameter, the lambda expression is not capturing
    // anything anymore, which allows the C# compiler to cache it.
    // Note that we can still access all private members of the
    // recipient from here, even if we're using the input parameter.
    recipient.SomeInstanceMethod();
});
```

💥 If you were directly referencing `Messenger.Default` to send messages (ie. outside of the `ObservableRecipient` class, which exposes a `Messenger` property which is unchanged), you'll need to replace that with either `WeakReferenceMessenger.Default` or `StrongReferenceMessenger.Default`, depending on the desired messenger to use.

💥 If you want to use the `StrongReferenceMessenger` class for better performance, make sure to pass that to the constructor of the `ObservableRecipient` class, otherwise the `WeakReferenceMessenger.Default` instance will be used.

💥 If you were using the `ObservableObject.SetProperty<T>(Expression<Func<T>>, ...)` overload, the code needs to be updated as follows to replace the LINQ expression with a stateless lambda expression:

```cs
private readonly User user;

public string Name
{
    // Preview 2
    set => SetProperty(() => user.Name, value);

    // Preview 3
    set => SetProperty(user.Name, value, user, (u, n) => u.Name = n);
}
```

The syntax is slightly more complex, but results in a 150x speed improvement (that's not a typo), requires no memory allocations at all and no reflection, and ensures that all necessary validation of the arguments can be done at compile time too.

💥 If you were using `SetPropertyAndNotifyOnCompletion`, change the code as follows:

```csharp
// Preview 2
private Task<string> myTask;

public Task<string> MyTask
{
     get => myTask;
     set => SetPropertyAndNotifyOnCompletion(ref myTask, () => myTask, value);
}

// Preview 3
private TaskNotifier<string> myTask;

public Task<string> MyTask
{
    get => myTask;
    set => SetPropertyAndNotifyOnCompletion(ref myTask, value);
}
```

💥 If you were using the `Ioc` class, we will provide docs to illustrate how to setup a custom service container using the `Microsoft.Extensions.DependencyInjection` library soon. The temporary link with the preview docs for this is available [here](https://github.com/windows-toolkit/MVVM-Samples/blob/feature/preview2-update/docs/mvvm/Ioc.md).

## Notes

For additional info and discussion, see the related issue [here](https://github.com/windows-toolkit/WindowsCommunityToolkit/issues/3428).
