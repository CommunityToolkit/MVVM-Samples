using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Input;

namespace MvvmSampleUwp.ViewModels
{
    public class AsyncRelayCommandPageViewModel : SamplePageViewModel
    {
        public AsyncRelayCommandPageViewModel()
        {
            DownloadTextCommand = new AsyncRelayCommand(DownloadTextAsync);
        }

        public IAsyncRelayCommand DownloadTextCommand { get; }

        private async Task<string> DownloadTextAsync()
        {
            await Task.Delay(3000); // Simulate a web request

            return "Hello world!";
        }
    }
}
