using CopterBot.Sensors;
using CopterBot.Sensors.Common;
using Microsoft.SPOT;

namespace CopterBot
{
    public class Program
    {
        public static void Main()
        {
            using (var barometer = new Barometer())
            {
                barometer.Init();

                Debug.Print(string.Concat("Temperature: ", barometer.GetTemperature().ToString("F1"), " *C"));
                Debug.Print(string.Concat("Pressure: ", barometer.GetPressure(), " Pa"));
                Debug.Print(string.Concat("Altitude: ", barometer.GetAltitude().ToString("F2"), " m"));
            }

            using (var compass = new Compass())
            {
                compass.Init(gainLevel: Gain.Level5);

                var directions = compass.GetDirections();

                Debug.Print(string.Concat("X: ", directions.X, " Y: ", directions.Y, " Z: ", directions.Z));
            }
        }
    }
}
