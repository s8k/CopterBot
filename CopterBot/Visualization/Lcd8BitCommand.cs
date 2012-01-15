namespace CopterBot.Visualization
{
    public enum Lcd8BitCommand : byte
    {
        /// <summary>
        /// 00000001
        /// </summary>
        CleanScreen = 0x01,

        /// <summary>
        /// 00000010
        /// </summary>
        ReturnCursorHome = 0x02,

        /// <summary>
        /// 00101000
        /// 
        /// 0  0  1  DL N  F  X  X
        /// DL: 8-bit/4-bit interface
        /// N: 2-line/1-line display mode
        /// F: 5x10 dots/5x8 dots font type
        /// </summary>
        SetTwoLinesMode = 0x28,

        /// <summary>
        /// 00001101
        /// 
        /// 0  0  0  0  1  D  C  B
        /// D: display on/off
        /// C: cursor on/off
        /// B: blinks on/off
        /// </summary>
        TurnOnDisplay = 0x0D,

        /// <summary>
        /// 00001000
        /// </summary>
        TurnOffDisplay = 0x08,

        /// <summary>
        /// 00000110
        /// 
        /// 0  0  0  0  0  1  ID  S
        /// ID: increment/decrement
        /// S: display shift/doesn't shift
        /// </summary>
        SetDisplayShift = 0x06,

        /// <summary>
        /// 11000000
        /// 
        /// 1  A6 A5 A4 A3 A2 A1 A0
        /// A0-6: address of the cell
        /// (0x40 - beginning of the second line)
        /// </summary>
        CursorToTheSecondLine = 0xC0
    }
}