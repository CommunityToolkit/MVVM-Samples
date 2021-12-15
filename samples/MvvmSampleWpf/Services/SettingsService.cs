using MvvmSample.Core.Services;
using System.Linq;

namespace MvvmSampleWpf.Services
{
    public class SettingsService : ISettingsService
    {
        public T? GetValue<T>(string key)
        {
            var property = Settings.Default.PropertyValues[key];
            if (property != null)
            {
                return (T?)property.PropertyValue;
            }

            return default(T);
        }

        public void SetValue<T>(string key, T value)
        {
            Settings.Default[key] = value;
            Settings.Default.Save();
        }
    }
}
