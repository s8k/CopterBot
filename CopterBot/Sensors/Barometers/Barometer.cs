using System;
using System.Threading;
using CopterBot.Common;
using CopterBot.Common.Interfaces;

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

        private readonly II2CBus bus = new I2CBus(Address, ClockRate, Timeout);
        
        private BarometerCalibrationData coefficients;
        private byte powerMode;

        public void Dispose()
        {
            bus.Dispose();
        }

        /// <summary>
        /// Puts the sensor into initial state.
        /// </summary>
        /// <param name="power">Power consumption/resolution mode.</param>
        public void Init(PowerMode power = PowerMode.Standard)
        {
            powerMode = (byte)power;
            ReadCalibrationData();
        }

        /// <summary>
        /// Gets temperature value in Celsius degrees.
        /// </summary>
        /// <returns></returns>
        public float GetTemperature()
        {
            return CompensateTemperature(ReadUncompensatedTemperature());
        }

        /// <summary>
        /// Gets atmospheric pressure value in pascals (Pa).
        /// </summary>
        /// <returns></returns>
        public Int32 GetPressure()
        {
            return CompensatePressure(ReadUncompensatedTemperature(), ReadUncompensatedPressure());
        }

        /// <summary>
        /// Gets true altitude value in meters (the elevation above mean sea level).
        /// </summary>
        /// <returns></returns>
        public float GetAltitude()
        {
            var pressure = GetPressure();
            var altitude = (float)(44330 * (1 - Math.Pow(pressure / SeaLevelPressure, 1 / 5.255f)));

            return altitude;
        }

        private void ReadCalibrationData()
        {
            var bytes = bus.ReadSequence(0xAA, 22);

            coefficients = new BarometerCalibrationData(bytes);
        }

        private Int32 ReadUncompensatedTemperature()
        {
            bus.Write(0xF4, 0x2E);
            
            Thread.Sleep(GetTemperatureMeasurementTimeout());

            return bus.ReadSequence(0xF6, 2).TwoMsbFirst();
        }

        private Int32 ReadUncompensatedPressure()
        {

            bus.Write(0xF4, (byte)(powerMode << 6 | 0x34));

            Thread.Sleep(GetPressureMeasurementTimeout(powerMode));

            return bus.ReadSequence(0xF6, 3).ThreeMsbFirst() >> (8 - powerMode);
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