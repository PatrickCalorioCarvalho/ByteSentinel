using ByteSentinelServer.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace ByteSentinelServer.Services
{

    public class AgentHub : Hub
    {
        private readonly MetricsService _service;

        public AgentHub(MetricsService service)
        {
            _service = service;
        }

        public async Task SendMetrics(AgentMetricsDto metrics)
        {
            await _service.SaveMetricsAsync(metrics);
        }
    }
}
