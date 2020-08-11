using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Uno.Extensions;
using Windows.UI;
#if !WINDOWS_UWP
using Uno.Foundation;
#endif
#if __WASM__
using Uno.Foundation.Interop;
#endif
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MvvmSampleUwp.Controls
{

    public abstract partial class JavaScriptControl :
#if !__WASM__
UserControl
#else
        Control, IJSObject
#endif
    {

#if !__WASM__
        private readonly WebView internalWebView;
#else
        private readonly JSObjectHandle _handle;
        JSObjectHandle IJSObject.Handle => _handle;
#endif

        public JavaScriptControl()
        {
#if !__WASM__
            Content = internalWebView = new WebView();
            internalWebView.DefaultBackgroundColor = Colors.Transparent;
            internalWebView.NavigationCompleted += NavigationCompleted;
#else
            _handle = JSObjectHandle.Create(this);
#endif
            Loaded += JavaScriptControl_Loaded;
        }

        private string HtmlContentId =>
#if __WASM__
            this.GetHtmlId();
#else
            "content";
#endif

        private void JavaScriptControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
#if !__WASM__
            var html = @"<html>
     <body>
       <div id = ""content""></ div>
    </ body>
    </ html>
";
            internalWebView.NavigateToString(html);
#else
            LoadJavaScript();
#endif
        }

        protected async Task LoadEmbeddedJavaScriptFile(string filename)
        {
            var markdownScript = (await GetEmbeddedFileStreamAsync(GetType(), filename)).ReadToEnd();

            await InvokeScriptAsync(markdownScript, false);
        }

        protected abstract Task LoadJavaScript();

#if !__WASM__
        private void NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            LoadJavaScript();
        }
#endif

        protected async Task UpdateHtmlFromScript(string contentScript)
        {
            if (Foreground is SolidColorBrush colorBrush)
            {
                var color = colorBrush.Color;
                // This is required because default tostring on wasm doesn't come out in the format #RRGGBB or even #AARRGGBB
                var colorString = $"#{color.R.ToString("X")}{color.G.ToString("X")}{color.B.ToString("X")}";
                Console.WriteLine($"Color {colorString}");
                var colorScript = $@"document.getElementById('{HtmlContentId}').style.color = '{colorString}';";
                await InvokeScriptAsync(colorScript);
            }

            var script = $@"document.getElementById('{HtmlContentId}').innerHTML = {contentScript};";
            await InvokeScriptAsync(script);
        }

        public async Task<string> InvokeScriptAsync(string scriptToRun, bool resizeAfterScript = true)
        {
            scriptToRun = ReplaceLiterals(scriptToRun);

#if !__WASM__
            var source = new CancellationTokenSource();
            var result = await internalWebView.InvokeScriptAsync(
#if !WINDOWS_UWP
                source.Token,
#endif
                "eval", new[] { scriptToRun }).AsTask();
            if (resizeAfterScript)
            {
                await ResizeToContent();
            }
            return result;
#else
            var script = $"javascript:eval(\"{scriptToRun}\");";
            Console.Error.WriteLine(script);

            try
            {
                var result = WebAssemblyRuntime.InvokeJS(script);
                Console.WriteLine($"Result: {result}");

            if (resizeAfterScript)
            {
                await ResizeToContent();
            }

                return result;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("FAILED " + e);
                return null;
            }

#endif
        }

        private static Func<string, string> ReplaceLiterals = txt =>
#if WINDOWS_UWP
        txt;
#else
        txt.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"").Replace("\'", "\\\'").Replace("`", "\\`").Replace("^", "\\^");
#endif


        public static async Task<Stream> GetEmbeddedFileStreamAsync(Type assemblyType, string fileName)
        {
            await Task.Yield();

            var manifestName = assemblyType.GetTypeInfo().Assembly
                .GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith(fileName.Replace(" ", "_").Replace("/", ".").Replace("\\", "."), StringComparison.OrdinalIgnoreCase));

            if (manifestName == null)
            {
                throw new InvalidOperationException($"Failed to find resource [{fileName}]");
            }

            return assemblyType.GetTypeInfo().Assembly.GetManifestResourceStream(manifestName);
        }
        public async Task ResizeToContent()
        {
            var documentRoot =
#if __WASM__
                $"document.getElementById('{HtmlContentId}')";
#else
                          $"document.body";
#endif


            var heightString = await InvokeScriptAsync($"{documentRoot}.scrollHeight.toString()",
                false);
            int height;
            if (int.TryParse(heightString, out height))
            {
                this.Height = height;
            }
        }
    }

    //public static class WebViewExtensions
    //{
    //    public static async Task ResizeToContent(this WebView webView)
    //    {
    //        var heightString = await webView.InvokeScriptAsync("eval", new[] { "document.body.scrollHeight.toString()" });
    //        int height;
    //        if (int.TryParse(heightString, out height))
    //        {
    //            webView.Height = height;
    //        }
    //    }
    //}
}
