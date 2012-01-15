namespace CopterBot.Sensors.Gyroscopes
{
    /// <summary>
    /// Digital low-pass filter bandwidth and internal sampling rate configuration.
    /// </summary>
    public enum Bandwidth : byte
    {
        /// <summary>
        /// 256Hz / 8kHz
        /// </summary>
        Hz256 = 0,

        /// <summary>
        /// 188Hz / 1kHz
        /// </summary>
        Hz188 = 1,

        /// <summary>
        /// 98Hz / 1kHz
        /// </summary>
        Hz98 = 2,

        /// <summary>
        /// 42Hz / 1kHz
        /// </summary>
        Hz42 = 3,

        /// <summary>
        /// 20Hz / 1kHz
        /// </summary>
        Hz20 = 4,

        /// <summary>
        /// 10Hz / 1kHz
        /// </summary>
        Hz10 = 5,

        /// <summary>
        /// 5Hz / 1kHz
        /// </summary>
        Hz5 = 6
    }
}