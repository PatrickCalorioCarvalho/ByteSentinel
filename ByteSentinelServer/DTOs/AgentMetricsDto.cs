namespace ByteSentinelServer.DTOs
{
    public class AgentMetricsDto
    {
        public Guid AgentId { get; set; }
        public SystemMetricsDto System { get; set; } = new();
        public List<ContainerDto> Containers { get; set; } = new();
    }
}
