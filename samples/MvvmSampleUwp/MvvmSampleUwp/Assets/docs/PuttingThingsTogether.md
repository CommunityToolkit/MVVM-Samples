---
title: PuttingThingsTogether
author: Sergio0694
description: An overview of how to combine different features of the MVVM Toolkit into a practical example
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, service, messenger, messaging, net core, net standard
dev_langs:
  - csharp
---

# Putting things together

Now that we've outline all the different components that are available through the `Microsoft.Toolkit.Mvvm` package, we can look at a practical example of them all coming together to build a single, larger example. In this case, we want to build a very simple and minimalistic Reddit browser for a select number of subreddits.

## What do we want to build

Let's start by outlining exactly what we want to build:

- A minimal Reddit browser made up of two "widgets": one showing posts from a subreddit, and the other one showing the currently selected post. The two widget need to be self contained and without strong references to one another.
- We want users to be able to select a subreddit from a list of available options, and we want to save the selected subreddit as a setting and load it up the next time the sample is loaded.
- We want the subreddit widget to also offer a refresh button to reload the current subreddit.
- For the purposes of this sample, we don't need to be able to handle all the possible post types. We'll just assign a sample text to all loaded posts and display that directly, to make things simpler.

## Setting up the viewmodels

Let's start with the viewmodel that will power the subreddit widget and let's go over the tools we need:

- **Commands:** we need the view to be able to request the viewmodel to reload the current list of posts from the selected subreddit. We can use the `AsyncRelayCommand` type to wrap a private method that will fetch the posts from Reddit. Here we're exposing the command through the `IAsyncRelayCommand` interface, to avoid strong references to the exact command type we're using. This will also allow us to potentially change the command type in the future without having to worry about any UI component relying on that specific type being used.
- **Properties:** we need to expose a number of values to the UI, which we can do with either observable properties if they're values we intend to completely replace, or with properties that are themselves observable (eg. `ObservableCollection<T>`). In this case, we have:
  - `ObservableCollection<object> Posts`, which is the observable list of loaded posts. Here we're just using `object` as a placeholder, as we haven't created a model to represent posts yet. We can replace this later on.
  - `IReadOnlyList<string> Subreddits`, which is a readonly list with the names of the subreddits that we allow users to choose from. This property is never updated, so it doesn't need to be observable either.
  - `string SelectedSubreddit`, which is the currently selected subreddit. This property needs to be bound to the UI, as it'll be used both to indicate the last selected subreddit when the sample is loaded, and to be manipulated directly from the UI as the user changes the selection. Here we're using the `SetProperty` method from the `ObservableObject` class.
  - `object SelectedPost`, which is the currently selected post. In this case we're using the `SetProperty` method from the `ObservableRecipient` class to indicate that we also want to broadcast notifications when this property changes. This is done to be able to notify the post widget that the current post selection is changed.
- **Methods:** we just need a private `LoadPostsAsync` method which will be wrapped by our async command, and which will contain the logic to load posts from the selected subreddit.

Here's the viewmodel so far:

```csharp
public sealed class SubredditWidgetViewModel : ObservableRecipient
{
    /// <summary>
    /// Creates a new <see cref="SubredditWidgetViewModel"/> instance.
    /// </summary>
    public SubredditWidgetViewModel()
    {
        LoadPostsCommand = new AsyncRelayCommand(LoadPostsAsync);
    }

    /// <summary>
    /// Gets the <see cref="IAsyncRelayCommand"/> instance responsible for loading posts.
    /// </summary>
    public IAsyncRelayCommand LoadPostsCommand { get; }

    /// <summary>
    /// Gets the collection of loaded posts.
    /// </summary>
    public ObservableCollection<object> Posts { get; } = new ObservableCollection<object>();

    /// <summary>
    /// Gets the collection of available subreddits to pick from.
    /// </summary>
    public IReadOnlyList<string> Subreddits { get; } = new[]
    {
        "microsoft",
        "windows",
        "surface",
        "windowsphone",
        "dotnet",
        "csharp"
    };

    private string selectedSubreddit;

    /// <summary>
    /// Gets or sets the currently selected subreddit.
    /// </summary>
    public string SelectedSubreddit
    {
        get => selectedSubreddit;
        set => SetProperty(ref selectedSubreddit, value);
    }

    private object selectedPost;

    /// <summary>
    /// Gets or sets the currently selected subreddit.
    /// </summary>
    public object SelectedPost
    {
        get => selectedPost;
        set => SetProperty(ref selectedPost, value, true);
    }

    /// <summary>
    /// Loads the posts from a specified subreddit.
    /// </summary>
    private async Task LoadPostsAsync()
    {
        // TODO...
    }
}
```

Now let's take a look at what we need for viewmodel of the post widget. This will be a much simpler viewmodel, as it really only needs to expose a `Post` property with the currently selected post, and to receive broadcast messages from the subreddit widget to update the `Post` property. It can look something like this:

```csharp
public sealed class PostWidgetViewModel : ObservableRecipient, IRecipient<PropertyChangedMessage<object>>
{
    private object post;

    /// <summary>
    /// Gets the currently selected post, if any.
    /// </summary>
    public object Post
    {
        get => post;
        private set => SetProperty(ref post, value);
    }

    /// <inheritdoc/>
    public void Receive(PropertyChangedMessage<object> message)
    {
        if (message.Sender.GetType() == typeof(SubredditWidgetViewModel) &&
            message.PropertyName == nameof(SubredditWidgetViewModel.SelectedPost))
        {
            Post = message.NewValue;
        }
    }
}
```

In this case, we're using the `IRecipient<TMessage>` interface to declare the messages we want our viewmodel to receive. The handlers for the declared messages will be added automatically by the `ObservableRecipient` class when the `IsActive` property is set to `true`. Note that it is not mandatory to use this approach, and manually registering each message handler is also possible, like so:

```csharp
public sealed class PostWidgetViewModel : ObservableRecipient
{
    protected override void OnActivated()
    {
        // We use a method group here, but a lambda expression is also valid
        Messenger.Register<PostWidgetViewModel, PropertyChangedMessage<object>>(this, (r, m) => r.Receive(m));
    }

    /// <inheritdoc/>
    public void Receive(PropertyChangedMessage<object> message)
    {
        if (message.Sender.GetType() == typeof(SubredditWidgetViewModel) &&
            message.PropertyName == nameof(SubredditWidgetViewModel.SelectedPost))
        {
            Post = message.NewValue;
        }
    }
}
```

We now have a draft of our viewmodels ready, and we can start looking into the services we need.

## Building the settings service

> [!NOTE]
> The sample is built using the dependency injection pattern, which is the recommended approach to deal with services in viewmodels. It is also possible to use other patterns, such as the service locator pattern, but the MVVM Toolkit does not offer built-in APIs to enable that.

Since we want some of our properties to be saved and persisted, we need a way for viewmodels to be able to interact with the application settings. We shouldn't use platform-specific APIs directly in our viewmodels though, as that would prevent us from having all our viewmodels in a portable, .NET Standard project. We can solve this issue by using services, and the APIs in the `Microsoft.Extensions.DependencyInjection` library to setup our `IServiceProvider` instance for the application. The idea is to write interfaces that represent all the API surface that we need, and then to implement platform-specific types implementing this interface on all our application targets. The viewmodels will only interact with the interfaces, so they will not have any strong reference to any platform-specific type at all.

Here's a simple interface for a settings service:

```csharp
public interface ISettingsService
{
    /// <summary>
    /// Assigns a value to a settings key.
    /// </summary>
    /// <typeparam name="T">The type of the object bound to the key.</typeparam>
    /// <param name="key">The key to check.</param>
    /// <param name="value">The value to assign to the setting key.</param>
    void SetValue<T>(string key, T value);

    /// <summary>
    /// Reads a value from the current <see cref="IServiceProvider"/> instance and returns its casting in the right type.
    /// </summary>
    /// <typeparam name="T">The type of the object to retrieve.</typeparam>
    /// <param name="key">The key associated to the requested object.</param>
    [Pure]
    T GetValue<T>(string key);
}
```

We can assume that platform-specific types implementing this interface will take care of dealing with all the logic necessary to actually serialize the settings, store them to disk and then read them back. We can now use this service in our `SubredditWidgetViewModel`, in order to make the `SelectedSubreddit` property persistent:

```csharp
/// <summary>
/// Gets the <see cref="ISettingsService"/> instance to use.
/// </summary>
private readonly ISettingsService SettingsService;

/// <summary>
/// Creates a new <see cref="SubredditWidgetViewModel"/> instance.
/// </summary>
public SubredditWidgetViewModel(ISettingsService settingsService)
{
    SettingsService = settingsService;

    selectedSubreddit = settingsService.GetValue<string>(nameof(SelectedSubreddit)) ?? Subreddits[0];
}

private string selectedSubreddit;

/// <summary>
/// Gets or sets the currently selected subreddit.
/// </summary>
public string SelectedSubreddit
{
    get => selectedSubreddit;
    set
    {
        SetProperty(ref selectedSubreddit, value);

        SettingsService.SetValue(nameof(SelectedSubreddit), value);
    }
}
```

Here we're using dependency injection and constructor injection, as mentioned above. We've declared an `ISettingsService SettingsService` field that just stores our settings service (which we're receiving as parameter in the viewmodel constructor), and then we're initializing the `SelectedSubreddit` property in the constructor, by either using the previous value or just the first available subreddit. Then we also modified the `SelectedSubreddit` setter, so that it will also use the settings service to save the new value to disk.

Great! Now we just need to write a platform specific version of this service, this time directly inside one of our app projects. Here's what that service might look like on UWP:

```csharp
public sealed class SettingsService : ISettingsService
{
    /// <summary>
    /// The <see cref="IPropertySet"/> with the settings targeted by the current instance.
    /// </summary>
    private readonly IPropertySet SettingsStorage = ApplicationData.Current.LocalSettings.Values;

    /// <inheritdoc/>
    public void SetValue<T>(string key, T value)
    {
        if (!SettingsStorage.ContainsKey(key)) SettingsStorage.Add(key, value);
        else SettingsStorage[key] = value;
    }

    /// <inheritdoc/>
    public T GetValue<T>(string key)
    {
        if (SettingsStorage.TryGetValue(key, out object value))
        {
            return (T)value;
        }

        return default;
    }
}
```

The final piece of the puzzle is to inject this platform-specific service into our service provider instance. We can do this at startup, like so:

```csharp
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

    services.AddSingleton<ISettingsService, SettingsService>();
    services.AddTransient<PostWidgetViewModel>();

    return services.BuildServiceProvider();
}
```

This will register a singleton instance of our `SettingsService` as a type implementing `ISettingsService`. We are also registering the `PostWidgetViewModel` as a transient service, meaning every time we retrieve an instance, it will be a new one (you can imagine this being useful if wanted to have multiple, independent post widgets). This means that every time we resolve an `ISettingsService` instance while the app in use is the UWP one, it will receive a `SettingsService` instance, which will use the UWP APIs behind the scene to manipulate settings. Perfect!

## Building the Reddit service

The last component of the backend that we're missing is a service that is able to use the Reddit REST APIs to fetch the posts from the subreddits we're interested in. To build it, we're going to use [refit](https://github.com/reactiveui/refit), which is a library to easily build type-safe services to interact with REST APIs. As before, we need to define the interface with all the APIs that our service will implement, like so:

```csharp
public interface IRedditService
{
    /// <summary>
    /// Get a list of posts from a given subreddit
    /// </summary>
    /// <param name="subreddit">The subreddit name.</param>
    [Get("/r/{subreddit}/.json")]
    Task<PostsQueryResponse> GetSubredditPostsAsync(string subreddit);
}
```

That `PostsQueryResponse` is a model we wrote that maps the JSON response for that API. The exact structure of that class is not important - suffice to say that it contains a collection of `Post` items, which are simple models representing our posts, that like like this:

```csharp
public class Post
{
    /// <summary>
    /// Gets or sets the title of the post.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the URL to the post thumbnail, if present.
    /// </summary>
    public string Thumbnail { get; set; }

    /// <summary>
    /// Gets the text of the post.
    /// </summary>
    public string SelfText { get; }
}
```

Once we have our service and our models, can plug them into our viewmodels to complete our backend. While doing so, we can also replace those `object` placeholders with the `Post` type we've defined:

```csharp
public sealed class SubredditWidgetViewModel : ObservableRecipient
{
    /// <summary>
    /// Gets the <see cref="IRedditService"/> instance to use.
    /// </summary>
    private readonly IRedditService RedditService = Ioc.Default.GetRequiredService<IRedditService>();

    /// <summary>
    /// Loads the posts from a specified subreddit.
    /// </summary>
    private async Task LoadPostsAsync()
    {
        var response = await RedditService.GetSubredditPostsAsync(SelectedSubreddit);

        Posts.Clear();

        foreach (var item in response.Data.Items)
        {
            Posts.Add(item.Data);
        }
    }
}
```

We have added a new `IRedditService` field to store our service, just like we did for the settings service, and we implemented our `LoadPostsAsync` method, which was previously empty.

The last missing piece now is just to inject the actual service into our service provider. The big difference in this case is that by using `refit` we don't actually need to implement the service at all! The library will automatically create a type implementing the service for us, behind the scenes. So we only need to get an `IRedditService` instance and inject it directly, like so:

```csharp
/// <summary>
/// Configures the services for the application.
/// </summary>
private static IServiceProvider ConfigureServices()
{
    var services = new ServiceCollection();

    services.AddSingleton<ISettingsService, SettingsService>();
    services.AddSingleton(RestService.For<IRedditService>("https://www.reddit.com/"));
    services.AddTransient<PostWidgetViewModel>();

    return services.BuildServiceProvider();
}
```

And that's all we need to do! We now have all our backend ready to use, including two custom services that we created specifically for this app! 🎉

## Building the UI

Now that all the backend is completed, we can write the UI for our widgets. Note how using the MVVM pattern let us focus exclusively on the business logic at first, without having to write any UI-related code until now. Here we'll remove all the UI code that's not interacting with our viewmodels, for simplicity, and we'll go through each different control one by one. The full source code can be found in the sample app.

Before going through the various controls, here's how we can resolve viewmodels for all the different views in our application (eg. the `PostWidgetView`):

```csharp
public PostWidgetView()
{
    this.InitializeComponent();
    this.DataContext = App.Current.Services.GetService<PostWidgetViewModel>();
}

public PostWidgetViewModel ViewModel => (PostWidgetViewModel)DataContext;
```

We're using our `IServiceProvider` instance to resolve the `PostWidgetViewModel` object we need, which is then assigned to the data context property. We're also creating a strongly-typed `ViewModel` property that simply casts the data context to the correct viewmodel type - this is needed to enable `x:Bind` in the XAML code.

Let's start with the subreddit widget, which features a `ComboBox` to select a subreddit, a `Button` to refresh the feed, a `ListView` to display posts and a `ProgressBar` to indicate when the feed is loading. We'll assume that the `ViewModel` property represents an instance of the viewmodel we've described before - this can be declared either in XAML or directly in code behind.

### Subreddit selector:

```xml
<ComboBox
    ItemsSource="{x:Bind ViewModel.Subreddits}"
    SelectedItem="{x:Bind ViewModel.SelectedSubreddit, Mode=TwoWay}">
    <interactivity:Interaction.Behaviors>
        <core:EventTriggerBehavior EventName="SelectionChanged">
            <core:InvokeCommandAction Command="{x:Bind ViewModel.LoadPostsCommand}"/>
        </core:EventTriggerBehavior>
    </interactivity:Interaction.Behaviors>
</ComboBox>
```

Here we're binding the source to the `Subreddits` property, and the selected item to the `SelectedSubreddit` property. Note how the `Subreddits` property is only bound once, as the collection itself sends change notifications, while the `SelectedSubreddit` property is bound with the `TwoWay` mode, as we need it both to be able to load the value we retrieve from our settings, as well as updating the property in the viewmodel when the user changes the selection. Additionally, we're using a XAML behavior to invoke our command whenever the selection changes.

### Refresh button:

```xml
<Button Command="{x:Bind ViewModel.LoadPostsCommand}"/>
```

This component is extremely simple, we're just binding our custom command to the `Command` property of the button, so that the command will be invoked whenever the user clicks on it.

### Posts list:

```xml
<ListView
    ItemsSource="{x:Bind ViewModel.Posts}"
    SelectedItem="{x:Bind ViewModel.SelectedPost, Mode=TwoWay}">
    <ListView.ItemTemplate>
        <DataTemplate x:DataType="models:Post">
            <Grid>
                <TextBlock Text="{x:Bind Title}"/>
                <controls:ImageEx Source="{x:Bind Thumbnail}"/>
            </Grid>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

Here we have a `ListView` binding the source and selection to our viewmodel property, and also a template used to display each post that is available. We're using `x:DataType` to enable `x:Bind` in our template, and we have two controls binding directly to the `Title` and `Thumbnail` properties of our post.

### Loading bar:

```xml
<ProgressBar Visibility="{x:Bind ViewModel.LoadPostsCommand.IsRunning, Mode=OneWay}"/>
```

Here we're binding to the `IsRunning` property, which is part of the `IAsyncRelayCommand` interface. The `AsyncRelayCommand` type will take care of raising notifications for that property whenever the asynchronous operation starts or completes for that command.

---

The last missing piece is the UI for the post widget. As before, we've removed all the UI-related code that was not necessary to interact with the viewmodels, for simplicity. The full source code is available in the sample app.

```xml
<Grid>

    <!--Header-->
    <Grid>
        <TextBlock Text="{x:Bind ViewModel.Post.Title, Mode=OneWay}"/>
        <controls:ImageEx  Source="{x:Bind ViewModel.Post.Thumbnail, Mode=OneWay}"/>
    </Grid>

    <!--Content-->
    <ScrollViewer>
        <TextBlock Text="{x:Bind ViewModel.Post.SelfText, Mode=OneWay}"/>
    </ScrollViewer>
</Grid>
```

Here we just have a header, with a `TextBlock` and an `ImageEx` control binding their `Text` and `Source` properties to the respective properties in our `Post` model, and a simple `TextBlock` inside a `ScrollViewer` that is used to display the (sample) content of the selected post.

## Good to go! 🚀

We've now built all our viewmodels, the necessary services, and the UI for our widgets - our simple Reddit browser is completed! This was just meant to be an example of how to build an app following the MVVM pattern and using the APIs from the MVVM Toolkit.

As stated above, this is only a reference, and you're free to modify this structure to fit your needs and/or to pick and choose only a subset of components from the library. Regardless of the approach you take, the MVVM Toolkit should provide a solid foundation to hit the ground running when starting a new application, by letting you focus on your business logic instead of having to worry about manually doing all the necessary plumbing to enable proper support for the MVVM pattern.