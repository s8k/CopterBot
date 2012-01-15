using System;
using CopterBot.Common;
using CopterBot.Common.Interfaces;

namespace CopterBot.Sensors.Gyroscopes
{
    /// <summary>
    /// ITG-3200 - Digital triaxial gyro sensor
    /// Specification: http://dl.dropbox.com/u/4052063/specs/ITG3200.pdf
    /// </summary>
    public class Gyroscope : IDisposable, ISleepable
    {
        private const byte Address = 0x68;
        private const byte ClockRate = 100;
        private const byte Timeout = 50;

        private readonly II2CBus bus = new I2CBus(Address, ClockRate, Timeout);

        public void Dispose()
        {
            bus.Dispose();
        }

        public void Sleep()
        {
            bus.Update(0x3E, value => (byte)(value | 0x40));
        }

        public void Wakeup()
        {
            bus.Update(0x3E, value => (byte)(value & 0xBF));
        }

        /// <summary>
        /// Puts the sensor into initial state.
        /// </summary>
        /// <param name="bandwidth">Digital low-pass filter configuration.</param>
        /// <param name="sampleRateDivider">Sample rate divider: Fsample = Finternal / (divider + 1).</param>
        public void Init(Bandwidth bandwidth = Bandwidth.Hz42, byte sampleRateDivider = 0)
        {
            bus.Write(0x3E, 0x00);
            bus.Write(0x15, sampleRateDivider);
            bus.Write(0x16, (byte)(0x18 | (byte)bandwidth));
            bus.Write(0x17, 0x00);
        }


        /// <summary>
        /// Gets gyro values in degree/second.
        /// </summary>
        public GyroData GetValuesByAxes()
        {
            var bytes = bus.ReadSequence(0x1D, 6);

            return new GyroData
                       {
                           X = bytes.TwoMsbFirst() / 14.375f,
                           Y = bytes.TwoMsbFirst(2) / 14.375f,
                           Z = bytes.TwoMsbFirst(4) / 14.375f
                       };
        }

        /// <summary>
        /// Gets temperature value in Celsius degrees.
        /// </summary>
        public float GetTemperature()
        {
            var bytes = bus.ReadSequence(0x1B, 2);

            return (bytes.TwoMsbFirst() + 13200) / 280f + 35;
        }
    }
}