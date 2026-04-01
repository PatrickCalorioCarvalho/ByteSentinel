namespace ByteSentinelServer.Models
{
    public class DiskMetric
    {
        public Guid Id { get; set; }
        public Guid SystemMetricId { get; set; }
        public string Name { get; set; } = "";
        public string MountPoint { get; set; } = "";
        public double TotalGB { get; set; }
        public double UsedGB { get; set; }
        public double UsagePercent { get; set; }
    }

}
