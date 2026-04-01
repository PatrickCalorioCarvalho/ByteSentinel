namespace ByteSentinelServer.Models
{
    public class ContainerMetric
    {
        public Guid Id { get; set; }
        public Guid AgentId { get; set; }
        public string ContainerId { get; set; } = "";
        public string Name { get; set; } = "";
        public string Status { get; set; } = "";
        public double Cpu { get; set; }
        public double Memory { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
