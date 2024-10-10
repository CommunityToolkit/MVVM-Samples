using System.Reflection;
using MvvmSample.Core.Services;

namespace MvvmSampleMAUI.Services;

public sealed class FileService : IFilesService
{
	public string InstallationPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

	public Task<Stream> OpenForReadAsync(string path)
	{
		if (GetType() is not Type type)
		{
			throw new InvalidOperationException("Could not retrieve type");
		}
		
		return GetEmbeddedFileStreamAsync(type, path);
	}


	static async Task<Stream> GetEmbeddedFileStreamAsync(Type assemblyType, string fileName)
	{
		var manifestName = assemblyType.GetTypeInfo().Assembly
				.GetManifestResourceNames()
				.FirstOrDefault(n => n.EndsWith(fileName.Replace(" ", "_").Replace("\\", ".").Replace("/", "."), StringComparison.OrdinalIgnoreCase))
			?? throw new InvalidOperationException($"Failed to find resource [{fileName}]");

		return await Task.FromResult(assemblyType.GetTypeInfo().Assembly.GetManifestResourceStream(manifestName) ?? Stream.Null)
			.ConfigureAwait(ConfigureAwaitOptions.ForceYielding | ConfigureAwaitOptions.None);
	}
}