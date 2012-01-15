using System;
using CopterBot.Common;
using CopterBot.Common.Interfaces;

namespace CopterBot.Sensors.Accelerometers
{
    /// <summary>
    /// BMA180 - Digital triaxial acceleration sensor
    /// Specification: http://dl.dropbox.com/u/4052063/specs/BMA180.pdf
    /// </summary>
    public class Accelerometer : IDisposable, ISleepable
    {
        private const byte Address = 0x41;
        private const byte ClockRate = 100;
        private const byte Timeout = 50;

        private readonly II2CBus bus = new I2CBus(Address, ClockRate, Timeout);

        private float scaleRange;

        public void Dispose()
        {
            bus.Dispose();
        }

        public void Sleep()
        {
            bus.Update(0x0D, value => (byte)(value | 0x02));
        }

        public void Wakeup()
        {
            bus.Update(0x0D, value => (byte)(value & 0xFD));
        }

        public void Init(ScaleRange scale = ScaleRange.G2, Bandwidth bandwidth = Bandwidth.Hz150)
        {
            EnableSettingsEditing();
            SetScaleRange((byte)scale);
            SetBandwidth((byte)bandwidth);
            BlockMsbWhileLsbIsRead();
        }

        public AccelerometerDirections GetDirections()
        {
            var bytes = bus.ReadSequence(0x02, 6);

            return new AccelerometerDirections
                       {
                           X = Adjust(bytes.TwoLsbFirst()),
                           Y = Adjust(bytes.TwoLsbFirst(2)),
                           Z = Adjust(bytes.TwoLsbFirst(4))
                       };
        }

        private void SetBandwidth(byte bandwidth)
        {
            bus.Update(0x20, value => (byte)(value & 0x0F | (bandwidth << 4)));
        }

        private void SetScaleRange(byte scale)
        {
            var scaleRangeMap = new[] { 1, 1.5f, 2, 3, 4, 8, 16 };
            scaleRange = scaleRangeMap[scale];

            bus.Update(0x35, value => (byte)(value & 0xF1 | (scale << 1)));
        }

        private void BlockMsbWhileLsbIsRead()
        {
            bus.Update(0x33, value => (byte)(value | 0x01));
        }

        private void EnableSettingsEditing()
        {
            bus.Update(0x0D, value => (byte)(value | 0x10));
        }

        private float Adjust(Int16 value)
        {
            var positive = (value & 0x8000) == 0x8000;
            var shifted = (Int16)(value >> 2);

            float result;

            if (positive)
            {
                result = (~shifted & 0x1FFF) - 1;
            }
            else
            {
                result = (shifted & 0x1FFF) * -1;
            }

            return result * scaleRange / 0x1FFF;
        }
    }
}