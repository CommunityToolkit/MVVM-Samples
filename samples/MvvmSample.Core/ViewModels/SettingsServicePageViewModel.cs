using MvvmSample.Core.Services;

namespace MvvmSample.Core.ViewModels
{
    public class SettingsServicePageViewModel : SamplePageViewModel
    {
        public SettingsServicePageViewModel(IFilesService filesService) : base(filesService)
        {
        }
    }
}
