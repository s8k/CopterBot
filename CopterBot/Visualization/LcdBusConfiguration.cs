using Microsoft.SPOT.Hardware;

namespace CopterBot.Visualization
{
    public class LcdBusConfiguration : ILcdBusConfiguration
    {
        public Cpu.Pin RegisterSelectPin { get; set; }

        public Cpu.Pin EnablePin { get; set; }

        public Cpu.Pin DataBit0Pin { get; set; }

        public Cpu.Pin DataBit1Pin { get; set; }

        public Cpu.Pin DataBit2Pin { get; set; }

        public Cpu.Pin DataBit3Pin { get; set; }
    }
}