using System;
using System.Threading;
using CopterBot.Common;
using CopterBot.Common.Interfaces;

namespace CopterBot.Sensors.Magnetometers
{
    /// <summary>
    /// HMC5883L - Digital triaxial compass
    /// Specification: http://dl.dropbox.com/u/4052063/specs/HMC5883L.pdf
    /// </summary>
    public class Compass : IDisposable
    {
        private const byte Address = 0x1E;
        private const byte ClockRate = 100;
        private const byte Timeout = 50;

        private readonly II2CBus bus = new I2CBus(Address, ClockRate, Timeout);

        public void Dispose()
        {
            bus.Dispose();
        }

        public void Init(Gain gainLevel = Gain.Level6)
        {
            bus.Write(0x01, (byte)((byte)gainLevel << 5));
        }
        
        public CompassDirections GetDirections()
        {
            PerformSingleMeasurement();

            var bytes = bus.ReadSequence(0x03, 6);

            return new CompassDirections
                       {
                           X = bytes.TwoMsbFirst(),
                           Y = bytes.TwoMsbFirst(4),
                           Z = bytes.TwoMsbFirst(2)
                       };
        }

        private void PerformSingleMeasurement()
        {
            bus.Write(0x02, 0x01);

            while (!IsReady())
            {
                Thread.Sleep(GetDirectionsMeasurementTimeout());
            }
        }

        private bool IsReady()
        {
            return (bus.Read(0x09) & 0x01) == 0x01;
        }

        private static int GetDirectionsMeasurementTimeout()
        {
            return 6;
        }
    }
}