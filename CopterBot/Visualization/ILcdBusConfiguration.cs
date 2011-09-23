using Microsoft.SPOT.Hardware;

namespace CopterBot.Visualization
{
    public interface ILcdBusConfiguration
    {
        Cpu.Pin RegisterSelectPin { get; }

        Cpu.Pin EnablePin { get; }

        Cpu.Pin DataBit0Pin { get; }

        Cpu.Pin DataBit1Pin { get; }

        Cpu.Pin DataBit2Pin { get; }

        Cpu.Pin DataBit3Pin { get; }
    }
}