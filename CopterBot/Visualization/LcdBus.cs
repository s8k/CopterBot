using Microsoft.SPOT.Hardware;

namespace CopterBot.Visualization
{
    public class LcdBus
    {
        public LcdBus(ILcdBusConfiguration configuration)
        {
            RegisterSelect = new OutputPort(configuration.Pin4, false);
            Enable = new OutputPort(configuration.Pin6, false);
            DataBit0 = new OutputPort(configuration.Pin11, false);
            DataBit1 = new OutputPort(configuration.Pin12, false);
            DataBit2 = new OutputPort(configuration.Pin13, false);
            DataBit3 = new OutputPort(configuration.Pin14, false);
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