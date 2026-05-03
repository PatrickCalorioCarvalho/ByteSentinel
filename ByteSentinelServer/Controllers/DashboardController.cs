using ByteSentinelServer.Data;
using ByteSentinelServer.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ByteSentinelServer.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("agents")]
        public async Task<IActionResult> GetAgentsStatus()
        {
            var agents = await _context.Agents.ToListAsync();

            var result = new List<AgentStatusDto>();

            foreach (var agent in agents)
            {
                var lastMetric = await _context.SystemMetrics
                    .Where(x => x.AgentId == agent.Id)
                    .OrderByDescending(x => x.Timestamp)
                    .FirstOrDefaultAsync();

                var containers = await _context.ContainerMetrics
                    .Where(x => x.AgentId == agent.Id)
                    .OrderByDescending(x => x.Timestamp)
                    .Take(10)
                    .ToListAsync();

                result.Add(new AgentStatusDto
                {
                    AgentId = agent.Id,
                    Name = agent.Name,
                    LastSeen = agent.LastSeen,
                    IsOnline = DateTime.UtcNow - agent.LastSeen < TimeSpan.FromSeconds(30),

                    CpuUsage = lastMetric?.CpuUsage ?? 0,
                    MemoryUsage = lastMetric?.MemoryUsage ?? 0,

                    ContainersRunning = containers.Count(x => x.Status.Contains("Up"))
                });
            }

            return Ok(result);
        }
        [HttpGet("agents/{agentId}/history")]
        public async Task<IActionResult> GetHistory(Guid agentId)
        {
            var data = await _context.SystemMetrics
                .Where(x => x.AgentId == agentId)
                .OrderByDescending(x => x.Timestamp)
                .Take(100)
                .Select(x => new
                {
                    x.Timestamp,
                    x.CpuUsage,
                    x.MemoryUsage
                })
                .ToListAsync();

            return Ok(data);
        }
        [HttpGet("agents/{agentId}/containers")]
        public async Task<IActionResult> GetContainers(Guid agentId)
        {
            var data = await _context.ContainerMetrics
                .Where(x => x.AgentId == agentId)
                .GroupBy(x => x.Name)
                .Select(g => g
                    .OrderByDescending(x => x.Timestamp)
                    .First())
                .ToListAsync();

            return Ok(data);
        }
    }
}
