using MvvmSample.Core.Services;

namespace MvvmSample.Core.ViewModels
{
    public class RedditServicePageViewModel : SamplePageViewModel
    {
        public RedditServicePageViewModel(IFilesService filesService) : base(filesService)
        {
        }
    }
}
