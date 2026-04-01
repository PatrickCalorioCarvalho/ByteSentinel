using ByteSentinelAgent.DTOs;
using Microsoft.AspNetCore.SignalR.Client;

namespace ByteSentinelAgent.Services
{

    public class SignalRService
    {
        private HubConnection _connection;
        private readonly IConfiguration _config;
        private readonly DockerService _docker;

        public SignalRService(IConfiguration config, DockerService docker)
        {
            _config = config;
            _docker = docker;
        }

        public async Task ConnectAsync()
        {
            var url = _config["Agent:ServerUrl"];
            var token = _config["Agent:Token"];

            _connection = new HubConnectionBuilder()
                .WithUrl(url, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .WithAutomaticReconnect()
                .Build();

            RegisterHandlers();

            await _connection.StartAsync();
        }

        private void RegisterHandlers()
        {
            _connection.On<string>("StartContainer", async (id) =>
            {
                await _docker.Start(id);
            });

            _connection.On<string>("StopContainer", async (id) =>
            {
                await _docker.Stop(id);
            });
        }

        public async Task SendMetrics(SystemMetricsDto system, List<ContainerDto> containers)
        {
            if (_connection.State == HubConnectionState.Connected)
            {
                var payload = new AgentMetricsDto
                {
                    AgentId = Guid.Parse(_config["Agent:AgentId"]), // importante
                    System = system,
                    Containers = containers
                };

                await _connection.InvokeAsync("SendMetrics", payload);
            }
        }
    }
}
