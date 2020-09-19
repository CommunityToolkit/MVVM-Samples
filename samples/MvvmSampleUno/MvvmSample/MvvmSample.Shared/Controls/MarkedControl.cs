using System;
using System.Threading.Tasks;
using Windows.UI.Text;
using Uno.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp.Helpers;

namespace MvvmSampleUwp.Controls
{
    public class MarkedControl
#if WINDOWS_UWP
        : Microsoft.Toolkit.Uwp.UI.Controls.MarkdownTextBlock
    {
        public MarkedControl()
        {
            Background = new SolidColorBrush("#60222222".ToColor());
            Padding = new Thickness(8);
            Header2FontWeight = FontWeights.Bold;
            Header2FontSize = 20;
            Header2Margin = new Thickness(0, 15, 0, 15);
            Header2Foreground = (Brush)Application.Current.Resources.ThemeDictionaries["DefaultTextForegroundThemeBrush"];
        }
    }
#else
        : JavaScriptControl
    {
        public event EventHandler MarkedReady;

        public bool IsMarkedReady { get; private set; }


        public string Text
        {
            get { return (string)GetValue(MarkdownTextProperty); }
            set { SetValue(MarkdownTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarkdownTextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MarkedControl), new PropertyMetadata(null, MarkdownTextChanged));

        private static async void MarkdownTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            await (d as MarkedControl).DisplayMarkdownText();
        }

        private async Task DisplayMarkdownText()
        {
            if (IsMarkedReady && !string.IsNullOrWhiteSpace(Text))
            {
                await DisplayMarkdown(Text);
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
#endif
}
