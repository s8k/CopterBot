namespace CopterBot.Visualization
{
    public static class LcdCommand
    {
        public const string Init = "0011";

        public const string Enable4BitInterface = "0010";

        public const string CleanScreen = "00000001";

        public const string ReturnCursorHome = "00000010";

        /// <summary>
        /// 0  0  1  DL N  F  X  X
        /// 
        /// DL: 8-bit/4-bit interface
        /// N: 2-line/1-line display mode
        /// F: 5x10 dots/5x8 dots font type
        /// </summary>
        public const string SetTwoLinesMode = "00101000";

        /// <summary>
        /// 0  0  0  0  1  D  C  B
        /// 
        /// D: display on/off
        /// C: cursor on/off
        /// B: blinks on/off
        /// </summary>
        public const string TurnOnDisplay = "00001101";

        /// <summary>
        /// 0  0  0  0  0  1  ID  S
        /// 
        /// ID: increment/decrement
        /// S: display shift/doesn't shift
        /// </summary>
        public const string SetDisplayShift = "00000110";

        /// <summary>
        /// 1  A6 A5 A4 A3 A2 A1 A0
        /// 
        /// A0-6: address of the cell
        /// (0x40 - beginning of the second line)
        /// </summary>
        public const string CursorToSecondLine = "11000000";
    }
}