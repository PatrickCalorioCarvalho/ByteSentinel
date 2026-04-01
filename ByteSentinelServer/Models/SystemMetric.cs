namespace ByteSentinelServer.Models
{
    public class SystemMetric
    {
        public Guid Id { get; set; }
        public Guid AgentId { get; set; }
        public double CpuUsage { get; set; }
        public double MemoryUsage { get; set; }
        public DateTime Timestamp { get; set; }

        public List<DiskMetric> Disks { get; set; } = new();
    }
}
