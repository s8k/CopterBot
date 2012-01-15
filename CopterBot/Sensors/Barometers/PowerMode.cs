namespace CopterBot.Sensors.Barometers
{
    /// <summary>
    /// Modes defining relation between power consumption, speed and measurement resolution.
    /// </summary>
    public enum PowerMode : byte
    {
        /// <summary>
        /// 1 sample, 4.5 ms
        /// </summary>
        UltraLow = 0,

        /// <summary>
        /// 2 samples, 7.5 ms
        /// </summary>
        Standard = 1,

        /// <summary>
        /// 4 samples, 13.5 ms
        /// </summary>
        High = 2,

        /// <summary>
        /// 8 samples, 25.5 ms
        /// </summary>
        UltraHigh = 3
    }
}