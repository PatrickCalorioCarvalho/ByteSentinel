using ByteSentinelServer.Data;
using ByteSentinelServer.DTOs;
using ByteSentinelServer.Models;

namespace ByteSentinelServer.Services
{
    public class MetricsService
    {
        private readonly AppDbContext _context;

        public MetricsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveMetricsAsync(AgentMetricsDto dto)
        {
            var agent = await _context.Agents.FindAsync(dto.AgentId);

            if (agent == null)
            {
                agent = new Agent
                {
                    Id = dto.AgentId,
                    Name = dto.AgentId.ToString(),
                    Hostname = "unknown"
                };
                _context.Agents.Add(agent);
            }

            agent.LastSeen = DateTime.UtcNow;
            agent.IsOnline = true;

            var systemMetric = new SystemMetric
            {
                Id = Guid.NewGuid(),
                AgentId = dto.AgentId,
                CpuUsage = dto.System.CpuUsage,
                MemoryUsage = dto.System.MemoryUsage,
                Timestamp = DateTime.UtcNow,
                Disks = dto.System.Disks.Select(d => new DiskMetric
                {
                    Id = Guid.NewGuid(),
                    Name = d.Name,
                    MountPoint = d.MountPoint,
                    TotalGB = d.TotalGB,
                    UsedGB = d.UsedGB,
                    UsagePercent = d.UsagePercent
                }).ToList()
            };

            _context.SystemMetrics.Add(systemMetric);

            var containers = dto.Containers.Select(c => new ContainerMetric
            {
                Id = Guid.NewGuid(),
                AgentId = dto.AgentId,
                ContainerId = c.Id,
                Name = c.Name,
                Status = c.Status,
                Cpu = c.Cpu,
                Memory = c.Memory,
                Timestamp = DateTime.UtcNow
            });

            _context.ContainerMetrics.AddRange(containers);

            await _context.SaveChangesAsync();
        }
    }
}
