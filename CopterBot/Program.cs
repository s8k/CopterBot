﻿using CopterBot.Sensors.Barometers;
using CopterBot.Sensors.Common;
using CopterBot.Sensors.Magnetometers;
using CopterBot.Visualization;
using GHIElectronics.NETMF.FEZ;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace CopterBot
{
    public class Program
    {
        public static void Main()
        {
            var lcdPorts = new LcdBus(
                (Cpu.Pin)FEZ_Pin.Digital.Di13,
                (Cpu.Pin)FEZ_Pin.Digital.Di11,
                (Cpu.Pin)FEZ_Pin.Digital.Di10,
                (Cpu.Pin)FEZ_Pin.Digital.Di9,
                (Cpu.Pin)FEZ_Pin.Digital.Di8,
                (Cpu.Pin)FEZ_Pin.Digital.Di7);

            using (var display = new Lcd(lcdPorts))
            {
                display.Init();
                display.Show("1All your base  are belong to us");
                display.Show1Line("first line");
                display.Show2Line("second line");
//                using (var barometer = new Barometer())
//                {
//                    barometer.Init();
//
//                    display.Show(string.Concat("Temperature: ", barometer.GetTemperature().ToString("F1"), " *C"));
//                    display.Show(string.Concat("Pressure: ", barometer.GetPressure(), " Pa"));
//                    display.Show(string.Concat("Altitude: ", barometer.GetAltitude().ToString("F2"), " m"));
//                }

//                using (var compass = new Compass())
//                {
//                    compass.Init(gainLevel: Gain.Level5);
//
//                    var directions = compass.GetDirections();
//
//                    display.Show(string.Concat("X: ", directions.X, " Y: ", directions.Y, " Z: ", directions.Z));
//                }
            }
        }
    }
}
