using MvvmSample.Core.Services;

namespace MvvmSampleMAUI.Services;

public sealed class SettingsService(IPreferences preferences) : ISettingsService
{
	readonly IPreferences _preferences = preferences;

	public T? GetValue<T>(string key) => _preferences.Get<T?>(key, default);

	public void SetValue<T>(string key, T? value) => _preferences.Set(key, value);
}