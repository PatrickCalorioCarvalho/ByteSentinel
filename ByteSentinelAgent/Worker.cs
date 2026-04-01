using ByteSentinelAgent.Services;

namespace ByteSentinelAgent
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        private readonly MetricsService _metrics;
        private readonly DockerService _docker;
        private readonly SignalRService _signalR;

        public Worker(
            ILogger<Worker> logger,
            IConfiguration config,
            MetricsService metrics,
            DockerService docker,
            SignalRService signalR)
        {
            _logger = logger;
            _config = config;
            _metrics = metrics;
            _docker = docker;
            _signalR = signalR;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var interval = TimeSpan.FromSeconds(
                _config.GetValue<int>("Agent:IntervalSeconds")
            );

            await _signalR.ConnectAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                var start = DateTime.UtcNow;

                try
                {
                    var system = await _metrics.GetSystemMetrics();
                    var containers = await _docker.GetContainers();

                    await _signalR.SendMetrics(system, containers);

                    _logger.LogInformation("Métricas enviadas com sucesso");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao coletar/enviar métricas");
                }

                var elapsed = DateTime.UtcNow - start;
                var delay = interval - elapsed;

                if (delay > TimeSpan.Zero)
                    await Task.Delay(delay, stoppingToken);
            }
        }
    }
}