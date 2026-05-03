using System;
using System.Collections.Generic;
using System.Text;

namespace ByteSentinelApp.DTOs
{
    public class ContainerDto
    {
        public string Name { get; set; } = "";
        public string Status { get; set; } = "";
        public double Cpu { get; set; }
        public double Memory { get; set; }
    }
}
