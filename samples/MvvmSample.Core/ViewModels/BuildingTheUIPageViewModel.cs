using MvvmSample.Core.Services;

namespace MvvmSample.Core.ViewModels
{
    public class BuildingTheUIPageViewModel : SamplePageViewModel
    {
        public BuildingTheUIPageViewModel(IFilesService filesService) : base(filesService)
        {
        }
    }
}
