namespace MvvmSampleMAUI;

static class AppResources
{
    public static T GetResource<T>(string resourceName, T? defaultValue = default)
    {
        if (Application.Current is null)
        {
            return defaultValue ?? throw new InvalidOperationException("Application cannot be null");
        }

        if (Application.Current.Resources.TryGetValue(resourceName, out var resource))
        {
            return (T)resource;
        }

        foreach (var resourceDictionary in Application.Current.Resources.MergedDictionaries)
        {
            if (resourceDictionary.TryGetValue(resourceName, out var resourceValue))
            {
                return (T)resourceValue;
            }
        }

        throw new KeyNotFoundException($"Resource {resourceName} not found");
    }
}