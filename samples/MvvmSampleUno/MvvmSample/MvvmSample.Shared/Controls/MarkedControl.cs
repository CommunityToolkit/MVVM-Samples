using System;
using System.Threading.Tasks;
using Uno.Extensions;
using Windows.UI.Xaml;

namespace MvvmSampleUwp.Controls
{
    public partial class MarkedControl : JavaScriptControl
    {
        public event EventHandler MarkedReady;

        public bool IsMarkedReady { get; private set; }


        public string MarkdownText
        {
            get { return (string)GetValue(MarkdownTextProperty); }
            set { SetValue(MarkdownTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MarkdownText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarkdownTextProperty =
            DependencyProperty.Register("MarkdownText", typeof(string), typeof(MarkedControl), new PropertyMetadata(null, MarkdownTextChanged));

        private static async void MarkdownTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            await (d as MarkedControl).DisplayMarkdownText();
        }

        private async Task DisplayMarkdownText()
        {
            if (IsMarkedReady && !string.IsNullOrWhiteSpace(MarkdownText))
            {
                await DisplayMarkdown(MarkdownText);
            }
        }

        public string MarkedEmbeddedJavaScriptFile { get; set; } = "marked.min.js";

        protected override async Task LoadJavaScript()
        {
            await LoadEmbeddedJavaScriptFile(MarkedEmbeddedJavaScriptFile);

            IsMarkedReady = true;

            await DisplayMarkdownText();

            MarkedReady?.Invoke(this, EventArgs.Empty);
        }

        public async Task DisplayMarkdown(string markdown)
        {
            markdown = markdown.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"").Replace("\'", "\\\'");//.Replace("\t","\\t").Replace("`","");
            System.Diagnostics.Debug.WriteLine(markdown);
            await UpdateHtmlFromScript($"marked('{markdown}')");
        }

        public async Task LoadMarkdownFromFile(string embeddedFileName)
        {
            var markdown = (await GetEmbeddedFileStreamAsync(GetType(), embeddedFileName)).ReadToEnd();
            await DisplayMarkdown(markdown);
        }
    }
}
