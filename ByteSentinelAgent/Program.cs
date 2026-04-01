using ByteSentinelAgent.Services;

namespace ByteSentinelAgent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            if (OperatingSystem.IsWindows())
            {
                builder.Services.AddWindowsService();
            }

            builder.Services.AddSingleton<MetricsService>();
            builder.Services.AddSingleton<DockerService>();
            builder.Services.AddSingleton<SignalRService>();

            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();

            host.Run();
        }
    }
}