using System.IO;
using System.Threading.Tasks;

namespace MvvmSampleUwp.Services
{
    /// <summary>
    /// The default <see langword="interface"/> for a service that handles files.
    /// </summary>
    public interface IFilesService
    {
        /// <summary>
        /// Gets the path of the installation directory.
        /// </summary>
        string InstallationPath { get; }

        /// <summary>
        /// Gets a readonly <see cref="Stream"/> for a file at a specified path.
        /// </summary>
        /// <param name="path">The path of the file to retrieve.</param>
        /// <returns>The <see cref="Stream"/> for the specified file.</returns>
        Task<Stream> OpenForReadAsync(string path);
    }
}
