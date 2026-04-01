
namespace ByteSentinelAgent.DTOs
{
    public class ContainerDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Status { get; set; }
        public double Cpu { get; set; }
        public double Memory { get; set; }
    }
}
