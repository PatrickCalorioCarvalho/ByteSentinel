using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace ByteSentinelApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine("🔥 UNHANDLED:");
                System.Diagnostics.Debug.WriteLine(e.ExceptionObject.ToString());
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine("🔥 TASK ERROR:");
                System.Diagnostics.Debug.WriteLine(e.Exception.ToString());
            };

            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .UseLiveCharts()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
