using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmSampleUwp.Helpers;

namespace MvvmSampleUwp.ViewModels
{
    /// <summary>
    /// A base class for viewmodels for sample pages in the app.
    /// </summary>
    public class SamplePageViewModel : ObservableObject
    {
        public SamplePageViewModel()
        {
            LoadDocsCommand = new AsyncRelayCommand<string>(LoadDocsAsync);
        }

        /// <summary>
        /// Gets the <see cref="IAsyncRelayCommand{T}"/> responsible for loading the source markdown docs.
        /// </summary>
        public IAsyncRelayCommand<string> LoadDocsCommand { get; }

        private IReadOnlyDictionary<string, string> texts;

        /// <summary>
        /// Gets the markdown for a specified paragraph from the docs page.
        /// </summary>
        /// <param name="key">The key of the paragraph to retrieve.</param>
        /// <returns>The text of the specified paragraph, or <see langword="null"/>.</returns>
        public string GetParagraph(string key)
        {
            return texts != null && texts.TryGetValue(key, out var value) ? value : string.Empty;
        }

        /// <summary>
        /// Implements the logic for <see cref="LoadDocsCommand"/>.
        /// </summary>
        /// <param name="name">The name of the docs file to load.</param>
        private async Task LoadDocsAsync(string name)
        {
            // Skip if the loading has already started
            if (!(LoadDocsCommand.ExecutionTask is null)) return;

            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/docs/{name}.md"));
            using var stream = await file.OpenStreamForReadAsync();
            using var reader = new StreamReader(stream);
            var text = await reader.ReadToEndAsync();

            texts = MarkdownHelper.GetParagraphs(text);

            OnPropertyChanged(nameof(GetParagraph));
        }
    }
}
