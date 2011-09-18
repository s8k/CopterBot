using System;
using Microsoft.SPOT.Hardware;

namespace CopterBot.Visualization
{
    public class LcdBus : IDisposable
    {
        public LcdBus(Cpu.Pin registerSelect, Cpu.Pin enable, Cpu.Pin dataBit0, Cpu.Pin dataBit1, Cpu.Pin dataBit2, Cpu.Pin dataBit3)
        {
            RegisterSelect = new OutputPort(registerSelect, false);
            Enable = new OutputPort(enable, false);
            DataBit0 = new OutputPort(dataBit0, false);
            DataBit1 = new OutputPort(dataBit1, false);
            DataBit2 = new OutputPort(dataBit2, false);
            DataBit3 = new OutputPort(dataBit3, false);
        }

        public void Dispose()
        {
            RegisterSelect.Dispose();
            Enable.Dispose();
            DataBit0.Dispose();
            DataBit1.Dispose();
            DataBit2.Dispose();
            DataBit3.Dispose();
        }

        public OutputPort RegisterSelect { get; private set; }

        public OutputPort Enable { get; private set; }

        public OutputPort DataBit0 { get; private set; }

        public OutputPort DataBit1 { get; private set; }

        public OutputPort DataBit2 { get; private set; }

        public OutputPort DataBit3 { get; private set; }
    }
}