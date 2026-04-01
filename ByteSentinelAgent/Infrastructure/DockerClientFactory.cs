using Docker.DotNet;

namespace ByteSentinelAgent.Infrastructure
{
    public static class DockerClientFactory
    {
        public static DockerClient Create()
        {
            var uri = OperatingSystem.IsWindows()
                ? "npipe://./pipe/docker_engine"
                : "unix:///var/run/docker.sock";

            return new DockerClientConfiguration(new Uri(uri)).CreateClient();
        }
    }
}


