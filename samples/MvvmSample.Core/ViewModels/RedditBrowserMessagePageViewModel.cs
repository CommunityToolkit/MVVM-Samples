using MvvmSample.Core.Services;

namespace MvvmSample.Core.ViewModels
{
    public class RedditBrowserMessagePageViewModel : SamplePageViewModel
    {
        public RedditBrowserMessagePageViewModel(IFilesService filesService) : base(filesService)
        {
        }
    }
}
