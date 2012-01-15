namespace CopterBot.Sensors.Accelerometers
{
    /// <summary>
    /// Full scale acceleration range.
    /// Important: The sensor is calibrated using 2g range. If using 8g and 16g range an offset correction is recommended.
    /// </summary>
    public enum ScaleRange : byte
    {
        /// <summary>
        /// +/- 1g | 0.13 mg/LSB
        /// </summary>
        G1 = 0,

        /// <summary>
        /// +/- 1.5g | 0.19 mg/LSB
        /// </summary>
        G1_5 = 1,

        /// <summary>
        /// +/- 2g | 0.25 mg/LSB
        /// </summary>
        G2 = 2,

        /// <summary>
        /// +/- 3g | 0.38 mg/LSB
        /// </summary>
        G3 = 3,

        /// <summary>
        /// +/- 4g | 0.50 mg/LSB
        /// </summary>
        G4 = 4,

        /// <summary>
        /// +/- 8g | 0.99 mg/LSB
        /// </summary>
        G8 = 5,

        /// <summary>
        /// +/- 16g | 1.98 mg/LSB
        /// </summary>
        G16 = 6
    }
}