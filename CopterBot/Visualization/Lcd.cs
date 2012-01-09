using System;
using System.Threading;

namespace CopterBot.Visualization
{
    /// <summary>
    /// SPLC780C - 16x2 dot-matrix LCD controller
    /// Specification: http://dl.dropbox.com/u/4052063/specs/SPLC780C.pdf
    /// </summary>
    public class Lcd : ILcd
    {
        private const int LineLength = 16;
        private readonly LcdBus bus;

        public Lcd(ILcdBusConfiguration configuration)
        {
            bus = new LcdBus(configuration);
        }

        public void Dispose()
        {
            //SendCommand(Lcd8BitCommand.TurnOffDisplay);
            bus.Dispose();
        }

        public void Init()
        {
            Thread.Sleep(50);
            for (byte i = 0; i < 3; i++)
            {
                SendCommand(Lcd4BitCommand.Init);
                Thread.Sleep(5);
            }

            SendCommand(Lcd4BitCommand.Enable4BitInterface);
            SendCommand(Lcd8BitCommand.CleanScreen);
            SendCommand(Lcd8BitCommand.ReturnCursorHome);
            SendCommand(Lcd8BitCommand.SetTwoLinesMode);
            SendCommand(Lcd8BitCommand.SetDisplayShift);
            SendCommand(Lcd8BitCommand.TurnOnDisplay);
        }

        public void Print(string text)
        {
            Print1Line(text);

            if (text.Length > LineLength)
            {
                SendCommand(Lcd8BitCommand.CursorToSecondLine);
                SendLine(text.Substring(LineLength));
            }
        }

        public void Print1Line(string text)
        {
            CheckArgument(text);
            SendCommand(Lcd8BitCommand.ReturnCursorHome);
            SendLine(text);
        }

        public void Print2Line(string text)
        {
            CheckArgument(text);
            SendCommand(Lcd8BitCommand.CursorToSecondLine);
            SendLine(text);
        }

        private void SendLine(string line)
        {
            for (byte i = 0; i < LineLength; i++)
            {
                if (i >= line.Length)
                {
                    SendAsciiCharacter(' ');
                    continue;
                }

                SendAsciiCharacter(line[i]);
            }
        }

        private static void CheckArgument(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
        }

        private void SendCommand(Lcd4BitCommand command)
        {
            EnterCommandMode();

            SetHigh4Bit((byte)command);
            PerformOperation();
        }

        private void SendCommand(Lcd8BitCommand command)
        {
            EnterCommandMode();

            SetHigh4Bit((byte)command);
            PerformOperation();

            SetLow4Bit((byte)command);
            PerformOperation();
        }

        private void SendAsciiCharacter(char ch)
        {
            EnterCharacterMode();

            SetHigh4Bit((byte)ch);
            PerformOperation();
            
            SetLow4Bit((byte)ch);
            PerformOperation();
        }

        private void SetHigh4Bit(byte value)
        {
            bus.DataBit3.Write(GetBit(value, 7));
            bus.DataBit2.Write(GetBit(value, 6));
            bus.DataBit1.Write(GetBit(value, 5));
            bus.DataBit0.Write(GetBit(value, 4));
        }

        private void SetLow4Bit(byte value)
        {
            bus.DataBit3.Write(GetBit(value, 3));
            bus.DataBit2.Write(GetBit(value, 2));
            bus.DataBit1.Write(GetBit(value, 1));
            bus.DataBit0.Write(GetBit(value, 0));
        }

        private void EnterCharacterMode()
        {
            bus.RegisterSelect.Write(true);
        }

        private void EnterCommandMode()
        {
            bus.RegisterSelect.Write(false);
        }

        private void PerformOperation()
        {
            bus.Enable.Write(true);
            Thread.Sleep(2);
            bus.Enable.Write(false);
        }

        private static bool GetBit(byte value, byte position)
        {
            var bit = 0x01 << position;

            return (value & bit) == bit;
        }
    }
}