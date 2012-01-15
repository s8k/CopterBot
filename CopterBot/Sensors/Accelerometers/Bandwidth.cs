namespace CopterBot.Sensors.Accelerometers
{
    /// <summary>
    /// Bandwidth defining type and quality of filters.
    /// </summary>
    public enum Bandwidth : byte
    {
        /// <summary>
        /// 10 Hz | Low-pass analog filter
        /// </summary>
        Hz10 = 0,

        /// <summary>
        /// 20 Hz | Low-pass analog filter
        /// </summary>
        Hz20 = 1,

        /// <summary>
        /// 40 Hz | Low-pass analog filter
        /// </summary>
        Hz40 = 2,

        /// <summary>
        /// 75 Hz | Low-pass analog filter
        /// </summary>
        Hz75 = 3,

        /// <summary>
        /// 150 Hz | Low-pass analog filter
        /// </summary>
        Hz150 = 4,

        /// <summary>
        /// 300 Hz | Low-pass analog filter
        /// </summary>
        Hz300 = 5,

        /// <summary>
        /// 600 Hz | Low-pass analog filter
        /// </summary>
        Hz600 = 6,

        /// <summary>
        /// 1200 Hz | Low-pass analog filter
        /// </summary>
        Hz1200 = 7,

        /// <summary>
        /// 1 Hz | High-pass digital filter
        /// </summary>
        HighPass = 8,

        /// <summary>
        /// 0.2 Hz .. 300 Hz | Band-pass digital filter
        /// </summary>
        BandPass = 9
    }
}