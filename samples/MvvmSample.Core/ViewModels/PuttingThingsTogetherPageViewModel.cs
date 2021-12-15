using MvvmSample.Core.Services;

namespace MvvmSample.Core.ViewModels
{
    public class PuttingThingsTogetherPageViewModel : SamplePageViewModel
    {
        public PuttingThingsTogetherPageViewModel(IFilesService filesService) : base(filesService)
        {
        }
    }
}
