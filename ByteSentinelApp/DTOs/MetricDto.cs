using System;
using System.Collections.Generic;
using System.Text;

namespace ByteSentinelApp.DTOs
{
    public class MetricDto
    {
        public DateTime Timestamp { get; set; }
        public double CpuUsage { get; set; }
        public double MemoryUsage { get; set; }
    }
}
