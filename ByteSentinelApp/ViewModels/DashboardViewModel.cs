using ByteSentinelApp.DTOs;
using ByteSentinelApp.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ByteSentinelApp.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _api = new();

        public ObservableCollection<ContainerDto> Containers { get; set; } = new();

        public ObservableCollection<AgentStatusDto> Agents { get; set; } = new();

        private AgentStatusDto _selectedAgent;
        public AgentStatusDto SelectedAgent
        {
            get => _selectedAgent;
            set
            {
                _selectedAgent = value;
                OnPropertyChanged();

                if (value != null)
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await LoadAgentData(value.AgentId);
                    });
            }
        }

        public ISeries[] Series { get; set; }

        private string _status;
        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        private Color _statusColor = Colors.Lime;
        public Color StatusColor
        {
            get => _statusColor;
            set { _statusColor = value; OnPropertyChanged(); }
        }

        private string _cpu;
        public string Cpu
        {
            get => _cpu;
            set { _cpu = value; OnPropertyChanged(); }
        }

        private string _ram;
        public string Ram
        {
            get => _ram;
            set { _ram = value; OnPropertyChanged(); }
        }

        private string _containersCount;
        public string ContainersCount
        {
            get => _containersCount;
            set { _containersCount = value; OnPropertyChanged(); }
        }
        public async Task LoadAgents()
        {
            var agents = await _api.GetAgents();

            Agents.Clear();
            foreach (var a in agents)
                Agents.Add(a);
        }

        private async Task LoadAgentData(Guid agentId)
        {
            var history = await _api.GetHistory(agentId);
            var containers = await _api.GetContainers(agentId);

            Containers.Clear();
            foreach (var c in containers)
                Containers.Add(c);

            ContainersCount = Containers.Count.ToString();

            var last = history.LastOrDefault();

            if (last != null)
            {
                Cpu = $"{last.CpuUsage:F1}%";
                Ram = $"{last.MemoryUsage:F1}%";
            }

            Status = Containers.Count > 0 ? "Online" : "Idle";

            StatusColor = Containers.Count switch
            {
                0 => Colors.Gray,
                < 3 => Colors.Lime,
                < 6 => Colors.Orange,
                _ => Colors.Red
            };

            Series = new ISeries[]
            {
            new LineSeries<double>
            {
                Values = history.Select(x => x.CpuUsage).ToArray(),
                Name = "CPU",
                GeometrySize = 6
            },
            new LineSeries<double>
            {
                Values = history.Select(x => x.MemoryUsage).ToArray(),
                Name = "RAM",
                GeometrySize = 6
            }
            };

            OnPropertyChanged(nameof(Series));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}