using MvvmSample.Core.Services;

namespace MvvmSample.Core.ViewModels
{
    public class IntroductionPageViewModel : SamplePageViewModel
    {
        public IntroductionPageViewModel(IFilesService filesService) : base(filesService)
        {
        }
    }
}
