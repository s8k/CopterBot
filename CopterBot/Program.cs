﻿using System.Threading;
using CopterBot.Sensors.Accelerometers;
using CopterBot.Sensors.Barometers;
using CopterBot.Sensors.Gyroscopes;
using CopterBot.Sensors.Magnetometers;
using CopterBot.Visualization;
using GHIElectronics.NETMF.FEZ;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Bandwidth = CopterBot.Sensors.Gyroscopes.Bandwidth;

namespace CopterBot
{
    public class Program
    {
        public static void Main()
        {
//            using (var compass = new Compass())
//            {
//                compass.Init(Gain.Max);
//
//                for (int i = 0; i < 15; i++)
//                {
//                    var directions = compass.GetVector();
//
//                    Debug.Print(string.Concat("X: ", directions.X, " Y: ", directions.Y, " Z: ", directions.Z));
//
//                    Thread.Sleep(500);
//                }
//            }

//            using (var accelerometer = new Accelerometer())
//            {
//                accelerometer.Init(ScaleRange.G2, Bandwidth.HighPass);
//                
//                for (int i = 0; i < 500; i++)
//                {
//                    var accelerometerDirections = accelerometer.GetVector();
//
//                    Debug.Print("X: " + accelerometerDirections.X.ToString("F2"));
//                    Debug.Print("Y: " + accelerometerDirections.Y.ToString("F2"));
//                    Debug.Print("Z: " + accelerometerDirections.Z.ToString("F2"));
//                    Debug.Print("--------------------");
//
//                    Thread.Sleep(500);
//                }
//            }

            using (var display = new Lcd(GetLcdBusConfiguration()))
            {
//                display.Init();
//
//                using (var gyroscope = new Gyroscope())
//                {
//                    gyroscope.Init(Bandwidth.Hz5);
//
//                    for (var i = 0; i < 200; i++)
//                    {
//                        var d = gyroscope.GetDataByAxes();
//
//                        display.Print1Line(string.Concat("X: ", d.X.ToString("F2")));
//                        display.Print2Line(string.Concat("Y: ", d.Y.ToString("F2"), " Z: ", d.Z.ToString("F2")));
//                    }
//
//
//                    gyroscope.Sleep();
//
//                    for (var i = 0; i < 200; i++)
//                    {
//                        var d = gyroscope.GetDataByAxes();
//
//                        display.Print1Line(string.Concat("X: ", d.X.ToString("F2")));
//                        display.Print2Line(string.Concat("Y: ", d.Y.ToString("F2"), " Z: ", d.Z.ToString("F2")));
//                    }
//
//
//                    gyroscope.Wakeup();
//
//                    for (var i = 0; i < 200; i++)
//                    {
//                        var d = gyroscope.GetDataByAxes();
//
//                        display.Print1Line(string.Concat("X: ", d.X.ToString("F2")));
//                        display.Print2Line(string.Concat("Y: ", d.Y.ToString("F2"), " Z: ", d.Z.ToString("F2")));
//                    }
//                }


                display.Init();
//                display.Print("All your base   are belong to us");
//                display.Print1Line("first line");
//                display.Print2Line("second line");


                using (var barometer = new Barometer())
                {
                    barometer.Init();

                    display.Print1Line(string.Concat("P: ", barometer.GetPressure(), " T: ", barometer.GetTemperature().ToString("F1")));
                    display.Print2Line(string.Concat("Alt: ", barometer.GetAltitude().ToString("F2"), " m"));
                }

//                using (var compass = new Compass())
//                {
//                    compass.Init(gainLevel: Gain.Level5);
//
//                    var directions = compass.GetVector();
//
//                    display.Print(string.Concat("X: ", directions.X, " Y: ", directions.Y, " Z: ", directions.Z));
//                }
            }
        }

        private static ILcdBusConfiguration GetLcdBusConfiguration()
        {
            return new LcdBusConfiguration
                       {
                           Pin4 = (Cpu.Pin)FEZ_Pin.Digital.Di13,
                           Pin6 = (Cpu.Pin)FEZ_Pin.Digital.Di11,
                           Pin11 = (Cpu.Pin)FEZ_Pin.Digital.Di10,
                           Pin12 = (Cpu.Pin)FEZ_Pin.Digital.Di9,
                           Pin13 = (Cpu.Pin)FEZ_Pin.Digital.Di8,
                           Pin14 = (Cpu.Pin)FEZ_Pin.Digital.Di7
                       };
        }
    }
}
