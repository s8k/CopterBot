namespace CopterBot.Sensors.Magnetometers
{
    /// <summary>
    /// Gain level configuration.
    /// Use higher gain level when you need more accurate measurement.
    /// </summary>
    public enum Gain : byte
    {
        /// <summary>
        /// +/- 8.1 Ga | 230 LSB/Gauss
        /// </summary>
        Min = 7,

        /// <summary>
        /// +/- 5.6 Ga | 330 LSB/Gauss
        /// </summary>
        Level1 = 6,

        /// <summary>
        /// +/- 4.7 Ga | 390 LSB/Gauss
        /// </summary>
        Level2 = 5,

        /// <summary>
        /// +/- 4.0 Ga | 440 LSB/Gauss
        /// </summary>
        Level3 = 4,

        /// <summary>
        /// +/- 2.5 Ga | 660 LSB/Gauss
        /// </summary>
        Level4 = 3,

        /// <summary>
        /// +/- 1.9 Ga | 820 LSB/Gauss
        /// </summary>
        Level5 = 2,

        /// <summary>
        /// +/- 1.3 Ga | 1090 LSB/Gauss
        /// </summary>
        Level6 = 1,

        /// <summary>
        /// +/- 0.88 Ga | 1370 LSB/Gauss
        /// </summary>
        Max = 0
    }
}