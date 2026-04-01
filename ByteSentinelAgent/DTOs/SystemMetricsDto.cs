namespace ByteSentinelAgent.DTOs
{
    public class SystemMetricsDto
    {
        public double CpuUsage { get; set; }
        public double MemoryUsage { get; set; }
        public List<DiskDto> Disks { get; set; } = [];
    }
}
