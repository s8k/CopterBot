using System;
using Microsoft.SPOT.Hardware;

namespace CopterBot.Visualization
{
    public interface ILcdBus : IDisposable
    {
        OutputPort RegisterSelect { get; }

        OutputPort Enable { get; }

        OutputPort DataBit0 { get; }

        OutputPort DataBit1 { get; }

        OutputPort DataBit2 { get; }

        OutputPort DataBit3 { get; }
    }
}