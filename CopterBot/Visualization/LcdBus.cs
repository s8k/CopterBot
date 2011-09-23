using Microsoft.SPOT.Hardware;

namespace CopterBot.Visualization
{
    public class LcdBus
    {
        public LcdBus(ILcdBusConfiguration configuration)
        {
            RegisterSelect = new OutputPort(configuration.RegisterSelectPin, false);
            Enable = new OutputPort(configuration.EnablePin, false);
            DataBit0 = new OutputPort(configuration.DataBit0Pin, false);
            DataBit1 = new OutputPort(configuration.DataBit1Pin, false);
            DataBit2 = new OutputPort(configuration.DataBit2Pin, false);
            DataBit3 = new OutputPort(configuration.DataBit3Pin, false);
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