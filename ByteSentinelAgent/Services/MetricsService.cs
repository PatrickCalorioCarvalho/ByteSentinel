
using ByteSentinelAgent.DTOs;
using System.Diagnostics;

namespace ByteSentinelAgent.Services
{
     public class MetricsService
    {
        public async Task<SystemMetricsDto> GetSystemMetrics()
        {
            return new SystemMetricsDto
            {
                CpuUsage = await GetCpuUsage(),
                MemoryUsage = GetMemoryUsage(),
                Disks = GetDisks()
            };
        }
        private List<DiskDto> GetDisks()
        {
            var disks = new List<DiskDto>();

            foreach (var drive in DriveInfo.GetDrives())
            {
                try
                {
                    if (!drive.IsReady)
                        continue;

                    var total = drive.TotalSize;
                    var free = drive.AvailableFreeSpace;
                    var used = total - free;

                    disks.Add(new DiskDto
                    {
                        Name = drive.Name,
                        MountPoint = drive.RootDirectory.FullName,
                        TotalGB = BytesToGB(total),
                        UsedGB = BytesToGB(used),
                        UsagePercent = Math.Round((double)used / total * 100, 2)
                    });
                }
                catch{ }
            }

            return disks;
        }
        private double BytesToGB(long bytes)
        {
            return Math.Round(bytes / 1024.0 / 1024 / 1024, 2);
        }
        private async Task<double> GetCpuUsage()
        {
            if (OperatingSystem.IsWindows())
            {
                var cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                cpu.NextValue();
                await Task.Delay(500);
                return Math.Round(cpu.NextValue(), 2);
            }
            else
            {
                var stat1 = File.ReadAllText("/proc/stat");
                await Task.Delay(500);
                var stat2 = File.ReadAllText("/proc/stat");

                var cpu1 = ParseCpu(stat1);
                var cpu2 = ParseCpu(stat2);

                var idle = cpu2.idle - cpu1.idle;
                var total = cpu2.total - cpu1.total;

                return Math.Round((1.0 - idle / total) * 100, 2);
            }
        }

        private (double idle, double total) ParseCpu(string stat)
        {
            var parts = stat.Split('\n')[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(double.Parse).ToArray();
            var idle = parts[3];
            var total = parts.Sum();
            return (idle, total);
        }

        private double GetMemoryUsage()
        {
            if (OperatingSystem.IsLinux())
            {
                var lines = File.ReadAllLines("/proc/meminfo");
                var total = double.Parse(lines[0].Split(':')[1].Trim().Split(' ')[0]);
                var free = double.Parse(lines[1].Split(':')[1].Trim().Split(' ')[0]);

                return Math.Round((1 - free / total) * 100, 2);
            }

            var info = GC.GetGCMemoryInfo();
            return info.HeapSizeBytes / 1024 / 1024;
        }
    }
}
