using System;
using CopterBot.Sensors.Common;
using Microsoft.SPOT.Hardware;

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

        private readonly I2CDevice device = new I2CDevice(new I2CDevice.Configuration(Address, ClockRate));

        private float scaling;

        public void Dispose()
        {
            device.Dispose();
        }

        public void Init(ScaleRange scaleRange = ScaleRange.G2, Bandwidth bandwidth = Bandwidth.Hz150)
        {
            EnableSettingsEditing();
            SetScaleRange((byte)scaleRange);
            SetBandwidth((byte)bandwidth);
            BlockMsbWhileLsbIsRead();
        }

        public void Sleep()
        {
            var registerBuffer = new byte[] { 0x0D };
            var readBuffer = new byte[1];

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(registerBuffer),
                                   I2CDevice.CreateReadTransaction(readBuffer)
                               },
                           Timeout);

            var newValue = (byte)(readBuffer[0] | 0x02);
            var toWrite = new byte[] { 0x0D, newValue };

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(toWrite)
                               },
                           Timeout);
        }

        public void Wakeup()
        {
            var registerBuffer = new byte[] { 0x0D };
            var readBuffer = new byte[1];

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(registerBuffer),
                                   I2CDevice.CreateReadTransaction(readBuffer)
                               },
                           Timeout);

            var newValue = (byte)(readBuffer[0] & 0xFD);
            var toWrite = new byte[] { 0x0D, newValue };

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(toWrite)
                               },
                           Timeout);
        }

        private void SetBandwidth(byte bandwidth)
        {
            var registerBuffer = new byte[] { 0x20 };
            var readBuffer = new byte[1];

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(registerBuffer),
                                   I2CDevice.CreateReadTransaction(readBuffer)
                               },
                           Timeout);

            var newValue = (byte)(readBuffer[0] & 0x0F | (bandwidth << 4));
            var toWrite = new byte[] { 0x20, newValue };

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(toWrite)
                               },
                           Timeout);
        }

        private void SetScaleRange(byte scaleRange)
        {
            var scaleRangeMap = new[] { 1, 1.5f, 2, 3, 4, 8, 16 };
            scaling = scaleRangeMap[scaleRange];

            var registerBuffer = new byte[] { 0x35 };
            var readBuffer = new byte[1];

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(registerBuffer),
                                   I2CDevice.CreateReadTransaction(readBuffer)
                               },
                           Timeout);

            var newValue = (byte)(readBuffer[0] & 0xF1 | (scaleRange << 1));
            var toWrite = new byte[] { 0x35, newValue };

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(toWrite)
                               },
                           Timeout);
        }

        private void BlockMsbWhileLsbIsRead()
        {
            var registerBuffer = new byte[] { 0x33 };
            var readBuffer = new byte[1];

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(registerBuffer),
                                   I2CDevice.CreateReadTransaction(readBuffer)
                               },
                           Timeout);

            var newValue = (byte)(readBuffer[0] | 0x01);
            var toWrite = new byte[] { 0x33, newValue };

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(toWrite)
                               },
                           Timeout);
        }

        private void EnableSettingsEditing()
        {
            var registerBuffer = new byte[] { 0x0D };
            var readBuffer = new byte[1];

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(registerBuffer),
                                   I2CDevice.CreateReadTransaction(readBuffer)
                               },
                           Timeout);

            var newValue = (byte)(readBuffer[0] | 0x10);
            var toWrite = new byte[] { 0x0D, newValue };

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(toWrite)
                               },
                           Timeout);
        }

        public AccelerometerDirections GetDirections()
        {
            var registerBuffer = new byte[] { 0x02 };
            var readBuffer = new byte[6];

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(registerBuffer),
                                   I2CDevice.CreateReadTransaction(readBuffer)
                               },
                           Timeout);

            return new AccelerometerDirections
                       {
                           X = Adjust(ByteCombiner.TwoLsbFirst(readBuffer)),
                           Y = Adjust(ByteCombiner.TwoLsbFirst(readBuffer, 2)),
                           Z = Adjust(ByteCombiner.TwoLsbFirst(readBuffer, 4))
                       };
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

            return result * scaling / 0x1FFF;
        }
    }
}