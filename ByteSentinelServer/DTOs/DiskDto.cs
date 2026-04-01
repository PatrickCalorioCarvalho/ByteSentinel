namespace ByteSentinelServer.DTOs
{
    public class DiskDto
    {
        public required string Name { get; set; }
        public required string MountPoint { get; set; }
        public double TotalGB { get; set; }
        public double UsedGB { get; set; }
        public double UsagePercent { get; set; }
    }
}
