using System;
using System.Threading;
using CopterBot.Sensors.Common;
using Microsoft.SPOT.Hardware;

namespace CopterBot.Sensors.Barometers
{
    /// <summary>
    /// BMP085 - Digital pressure sensor
    /// Specification: http://dl.dropbox.com/u/4052063/specs/BMP085.pdf
    /// </summary>
    public class Barometer : IDisposable
    {
        private const byte Address = 0x77;
        private const byte ClockRate = 100;
        private const byte Timeout = 50;
        private const float SeaLevelPressure = 101325;

        private readonly I2CDevice device = new I2CDevice(new I2CDevice.Configuration(Address, ClockRate));
        
        private BarometerCalibrationData coefficients;
        private byte powerMode;

        public void Dispose()
        {
            device.Dispose();
        }

        public void Init(PowerMode power = PowerMode.Standard)
        {
            powerMode = (byte)power;

            var readRegister = new byte[] { 0xAA };
            var readBuffer = new byte[22];

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(readRegister),
                                   I2CDevice.CreateReadTransaction(readBuffer)
                               },
                           Timeout);

            coefficients = new BarometerCalibrationData(readBuffer);
        }

        public float GetTemperature()
        {
            return CompensateTemperature(ReadUncompensatedTemperature());
        }

        public Int32 GetPressure()
        {
            return CompensatePressure(ReadUncompensatedTemperature(), ReadUncompensatedPressure());
        }

        public float GetAltitude()
        {
            var pressure = GetPressure();
            var altitude = (float)(44330 * (1 - Math.Pow(pressure / SeaLevelPressure, 1 / 5.255f)));

            return altitude;
        }

        private Int32 ReadUncompensatedTemperature()
        {
            var writeBuffer = new byte[] { 0xF4, 0x2E };
            var readRegister = new byte[] { 0xF6 };
            var readBuffer = new byte[2];

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(writeBuffer)
                               },
                           Timeout);

            Thread.Sleep(GetTemperatureMeasurementTimeout());

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(readRegister),
                                   I2CDevice.CreateReadTransaction(readBuffer)
                               },
                           Timeout);

            return ByteCombiner.TwoMsbFirst(readBuffer);
        }

        private Int32 ReadUncompensatedPressure()
        {
            var writeRegister = new byte[] { 0xF4, (byte)(powerMode << 6 | 0x34) };
            var readRegister = new byte[] { 0xF6 };
            var readBuffer = new byte[3];

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(writeRegister)
                               },
                           Timeout);

            Thread.Sleep(GetPressureMeasurementTimeout(powerMode));

            device.Execute(new I2CDevice.I2CTransaction[]
                               {
                                   I2CDevice.CreateWriteTransaction(readRegister),
                                   I2CDevice.CreateReadTransaction(readBuffer)
                               },
                           Timeout);

            return ByteCombiner.ThreeMsbFirst(readBuffer) >> (8 - powerMode);
        }

        private Int32 GetTemperatureCoefficient(Int32 uncompensatedTemperature)
        {
            var x1 = (uncompensatedTemperature - coefficients.Ac6) * coefficients.Ac5 >> 15;
            var x2 = (coefficients.Mc << 11) / (x1 + coefficients.Md);
            return x1 + x2;
        }

        private float CompensateTemperature(Int32 uncompensatedTemperature)
        {
            var b5 = GetTemperatureCoefficient(uncompensatedTemperature);
            var t = (float)((b5 + 8) >> 4) / 10;

            return t;
        }

        private Int32 CompensatePressure(Int32 uncompensatedTemperature, Int32 uncompensatedPressure)
        {
            var b5 = GetTemperatureCoefficient(uncompensatedTemperature);
            var b6 = b5 - 4000;
            var x1 = (coefficients.B2 * (b6 * b6 >> 12)) >> 11;
            var x2 = coefficients.Ac2 * b6 >> 11;
            var x3 = x1 + x2;
            var b3 = ((((coefficients.Ac1 << 2) + x3) << powerMode) + 2) >> 2;

            x1 = coefficients.Ac3 * b6 >> 13;
            x2 = (coefficients.B1 * (b6 * b6 >> 12)) >> 16;
            x3 = (x1 + x2 + 2) >> 2;
            var b4 = coefficients.Ac4 * (UInt32)(x3 + 32768) >> 15;
            var b7 = (UInt32)(uncompensatedPressure - b3) * (UInt32)(50000 >> powerMode);

            var p = (Int32)(b7 < 0x80000000 ? (b7 * 2) / b4 : (b7 / b4) * 2);

            x1 = (p >> 8) * (p >> 8);
            x1 = (3038 * x1) >> 16;
            x2 = (-7357 * p) >> 16;
            p = p + ((x1 + x2 + 3791) >> 4);

            return p;
        }

        private static Int32 GetPressureMeasurementTimeout(byte oversampling)
        {
            return 2 + (3 << oversampling);
        }

        private static Int32 GetTemperatureMeasurementTimeout()
        {
            return 5;
        }
    }
}