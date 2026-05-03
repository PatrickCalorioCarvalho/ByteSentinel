
namespace ByteSentinelApp.DTOs
{
    public class AgentStatusDto
    {
        public Guid AgentId { get; set; }
        public string Name { get; set; } = "";
        public bool IsOnline { get; set; }
        public double CpuUsage { get; set; }
        public double MemoryUsage { get; set; }
        public int ContainersRunning { get; set; }
    }
}
