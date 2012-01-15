namespace CopterBot.Common.Interfaces
{
    public interface ISleepable
    {
        /// <summary>
        /// Puts the device into low power sleep mode.
        /// </summary>
        void Sleep();

        /// <summary>
        /// Puts the device into normal operating mode.
        /// </summary>
        void Wakeup();
    }
}