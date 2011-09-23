using Microsoft.SPOT.Hardware;

namespace CopterBot.Visualization
{
    public interface ILcdBusConfiguration
    {
        Cpu.Pin Pin4 { get; }

        Cpu.Pin Pin6 { get; }

        Cpu.Pin Pin11 { get; }

        Cpu.Pin Pin12 { get; }

        Cpu.Pin Pin13 { get; }

        Cpu.Pin Pin14 { get; }
    }
}