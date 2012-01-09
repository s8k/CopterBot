namespace CopterBot.Visualization
{
    public enum Lcd4BitCommand : byte
    {
        /// <summary>
        /// 0011
        /// </summary>
        Init = 0x03,

        /// <summary>
        /// 0010
        /// </summary>
        Enable4BitInterface = 0x02
    }
}