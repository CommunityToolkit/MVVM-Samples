using MvvmSample.Core.Services;

namespace MvvmSample.Core.ViewModels
{
    public class SettingUpTheViewModelsPageViewModel : SamplePageViewModel
    {
        public SettingUpTheViewModelsPageViewModel(IFilesService filesService) : base(filesService)
        {
        }
    }
}
