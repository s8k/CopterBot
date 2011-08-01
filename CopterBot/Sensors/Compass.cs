using System;
using System.Threading;
using CopterBot.Sensors.Common;
using Microsoft.SPOT.Hardware;

namespace CopterBot.Sensors
{
    /// <summary>
    /// HMC5883L - 3-Axis Digital Compass
    /// </summary>
    public class Compass : IDisposable
    {
        private const UInt16 Address = 0x1E;
        private const int ClockRate = 100;
        private const int Timeout = 50;

        private readonly I2CDevice device = new I2CDevice(new I2CDevice.Configuration(Address, ClockRate));

        public void Dispose()
        {
            device.Dispose();
        }

        public void Init(Oversampling oversamplingLevel = Oversampling.Level1, Gain gainLevel = Gain.Level1)
        {
            var configRegister1 = new byte[] { 0x00, (byte)((byte)oversamplingLevel << 5 | 0x10) };
            var configRegister2 = new byte[] { 0x01, (byte)((byte)gainLevel << 5) };

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(configRegister1),
                                   I2CDevice.CreateWriteTransaction(configRegister2)
                               },
                           Timeout);
        }
        
        public CompassDirections GetDirections()
        {
            PerformSingleMeasurement();
            
            var directions = ReadDirections();

            return new CompassDirections
                       {
                           X = ByteCombiner.TwoBytes(directions),
                           Y = ByteCombiner.TwoBytes(directions, 4),
                           Z = ByteCombiner.TwoBytes(directions, 2)
                       };
        }

        private void PerformSingleMeasurement()
        {
            var modeRegister = new byte[] { 0x02, 0x01 };

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(modeRegister)
                               },
                           Timeout);

            while (!IsReady())
            {
                Thread.Sleep(GetDirectionsMeasurementTimeout());
            }
        }

        private bool IsReady()
        {
            var registerBuffer = new byte[] { 0x09 };
            var readBuffer = new byte[1];

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(registerBuffer),
                                   I2CDevice.CreateReadTransaction(readBuffer)
                               },
                           Timeout);

            return (readBuffer[0] & 0x01) == 0x01;
        }

        private byte[] ReadDirections()
        {
            var registerBuffer = new byte[] { 0x03 };
            var readBuffer = new byte[6];

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(registerBuffer),
                                   I2CDevice.CreateReadTransaction(readBuffer)
                               },
                           Timeout);

            return readBuffer;
        }

        private static int GetDirectionsMeasurementTimeout()
        {
            return 6;
        }
    }
}