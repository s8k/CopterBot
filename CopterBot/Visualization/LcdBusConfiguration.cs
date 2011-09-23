using Microsoft.SPOT.Hardware;

namespace CopterBot.Visualization
{
    public class LcdBusConfiguration : ILcdBusConfiguration
    {
        public Cpu.Pin Pin4 { get; set; }

        public Cpu.Pin Pin6 { get; set; }

        public Cpu.Pin Pin11 { get; set; }

        public Cpu.Pin Pin12 { get; set; }

        public Cpu.Pin Pin13 { get; set; }

        public Cpu.Pin Pin14 { get; set; }
    }
}