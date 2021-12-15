using MvvmSample.Core.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MvvmSampleWpf.Services
{
    public class FilesService : IFilesService
    {
        public string InstallationPath => Environment.CurrentDirectory;

        public Task<Stream> OpenForReadAsync(string path)
        {
            Stream result = File.OpenRead(path);
            return Task.FromResult(result);
        }
    }
}
