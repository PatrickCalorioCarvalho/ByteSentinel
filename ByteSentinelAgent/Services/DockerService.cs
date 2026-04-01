using ByteSentinelAgent.Infrastructure;
using ByteSentinelAgent.DTOs;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace ByteSentinelAgent.Services
{

    public class DockerService
    {
        private readonly DockerClient _client;

        public DockerService()
        {
            _client = DockerClientFactory.Create();
        }

        public async Task<List<ContainerDto>> GetContainers()
        {
            var list = await _client.Containers.ListContainersAsync(
                new ContainersListParameters { All = true });

            return list.Select(c => new ContainerDto
            {
                Id = c.ID,
                Name = c.Names.FirstOrDefault()?.Trim('/') ?? "SEM NOME",
                Status = c.Status
            }).ToList();
        }

        public async Task Start(string id)
        {
            await _client.Containers.StartContainerAsync(id, null);
        }

        public async Task Stop(string id)
        {
            await _client.Containers.StopContainerAsync(id, new ContainerStopParameters());
        }
    }
}
