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

        /// <summary>
        /// Puts the sensor into initial state.
        /// </summary>
        /// <param name="gainLevel">Gain level configuration.</param>
        public void Init(Gain gainLevel = Gain.Level6)
        {
            bus.Write(0x01, (byte)((byte)gainLevel << 5));
        }
        
        /// <summary>
        /// Gets magnetic vector.
        /// Output range: 0xF800 – 0x07FF (-2048 – 2047).
        /// Important: If there is a math overflow during the bias measurement, faulty values will be equal to -4096.
        /// </summary>
        public MagneticVector GetVector()
        {
            PerformSingleMeasurement();

            var bytes = bus.ReadSequence(0x03, 6);

            return new MagneticVector
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