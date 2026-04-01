namespace ByteSentinelServer.Models
{
    public class Agent
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Hostname { get; set; } = "";
        public DateTime LastSeen { get; set; }
        public bool IsOnline { get; set; }
    }
}
