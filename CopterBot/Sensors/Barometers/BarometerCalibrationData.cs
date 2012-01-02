using System;
using CopterBot.Sensors.Common;

namespace CopterBot.Sensors.Barometers
{
    public class BarometerCalibrationData
    {
        private readonly Int16[] coefficients = new Int16[11];

        public BarometerCalibrationData(byte[] data)
        {
            for (byte i = 0; i < coefficients.Length; i++)
            {
                coefficients[i] = ByteCombiner.TwoMsbFirst(data, i * 2);
            }
        }

        public Int16 Ac1
        {
            get { return coefficients[0]; }
        }

        public Int16 Ac2
        {
            get { return coefficients[1]; }
        }

        public Int16 Ac3
        {
            get { return coefficients[2]; }
        }

        public UInt16 Ac4
        {
            get { return (UInt16)coefficients[3]; }
        }

        public UInt16 Ac5
        {
            get { return (UInt16)coefficients[4]; }
        }

        public UInt16 Ac6
        {
            get { return (UInt16)coefficients[5]; }
        }

        public Int16 B1
        {
            get { return coefficients[6]; }
        }

        public Int16 B2
        {
            get { return coefficients[7]; }
        }

        public Int16 Mb
        {
            get { return coefficients[8]; }
        }

        public Int16 Mc
        {
            get { return coefficients[9]; }
        }

        public Int16 Md
        {
            get { return coefficients[10]; }
        }
    }
}
