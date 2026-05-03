using ByteSentinelApp.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
 
namespace ByteSentinelApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri("http://192.168.18.49:55000")
        };

        public async Task<List<AgentStatusDto>> GetAgents()
        {
            var result = await _http.GetFromJsonAsync<List<AgentStatusDto>>("/api/dashboard/agents");
            return result ?? new List<AgentStatusDto>();
        }
        public async Task<List<MetricDto>> GetHistory(Guid agentId)
        {
            return await _http.GetFromJsonAsync<List<MetricDto>>($"/api/dashboard/agents/{agentId}/history")
                   ?? new();
        }

        public async Task<List<ContainerDto>> GetContainers(Guid agentId)
        {
            return await _http.GetFromJsonAsync<List<ContainerDto>>($"/api/dashboard/agents/{agentId}/containers")
                   ?? new();
        }
    }
}
