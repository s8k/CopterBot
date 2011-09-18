using System;
using System.Threading;

namespace CopterBot.Visualization
{
    /// <summary>
    /// SPLC780C - 16x2 dot-matrix LCD controller
    /// Specification: http://dl.dropbox.com/u/4052063/specs/SPLC780C.pdf
    /// </summary>
    public class Lcd : IDisposable
    {
        private const int LineLength = 16;
        private readonly LcdBus bus;

        public Lcd(LcdBus bus)
        {
            this.bus = bus;
        }

        public void Dispose()
        {
            bus.Dispose();
        }

        public void Init()
        {
            Thread.Sleep(50);
            for (byte i = 0; i < 3; i++)
            {
                SendCommand(LcdCommand.Init);
                Thread.Sleep(5);
            }

            SendCommand(LcdCommand.Enable4BitInterface);
            SendCommand(LcdCommand.SetTwoLinesMode);
            SendCommand(LcdCommand.CleanScreen);
            SendCommand(LcdCommand.TurnOnDisplay);
            SendCommand(LcdCommand.SetDisplayShift);
            SendCommand(LcdCommand.ReturnCursorHome);
        }

        public void Show(string text)
        {
            Show1Line(text);

            if (text.Length > LineLength)
            {
                SendCommand(LcdCommand.CursorToSecondLine);
                SendLine(text.Substring(LineLength));
            }
        }

        public void Show1Line(string text)
        {
            CheckArgument(text);
            SendCommand(LcdCommand.ReturnCursorHome);
            SendLine(text);
        }

        public void Show2Line(string text)
        {
            CheckArgument(text);
            SendCommand(LcdCommand.CursorToSecondLine);
            SendLine(text);
        }

        private void SendLine(string line)
        {
            for (byte i = 0; i < LineLength; i++)
            {
                if (i >= line.Length)
                {
                    SendCharacter(' ');
                    continue;
                }

                SendCharacter(line[i]);
            }
        }

        private static void CheckArgument(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
        }

        private void SendCommand(string command)
        {
            EnterCommandMode();

            SetHigh4Bit(command);
            PerformOperation();

            if (command.Length == 8)
            {
                SetLow4Bit(command);
                PerformOperation();
            }
        }

        private void SendCharacter(char ch)
        {
            EnterCharacterMode();
            SetHigh4Bit(ch);
            PerformOperation();
            SetLow4Bit(ch);
            PerformOperation();
        }

        private void SetHigh4Bit(string command)
        {
            bus.DataBit3.Write(command[0] == '1');
            bus.DataBit2.Write(command[1] == '1');
            bus.DataBit1.Write(command[2] == '1');
            bus.DataBit0.Write(command[3] == '1');
        }

        private void SetLow4Bit(string command)
        {
            bus.DataBit3.Write(command[4] == '1');
            bus.DataBit2.Write(command[5] == '1');
            bus.DataBit1.Write(command[6] == '1');
            bus.DataBit0.Write(command[7] == '1');
        }

        private void SetHigh4Bit(char ch)
        {
            bus.DataBit3.Write((ch & 0x80) == 0x80);
            bus.DataBit2.Write((ch & 0x40) == 0x40);
            bus.DataBit1.Write((ch & 0x20) == 0x20);
            bus.DataBit0.Write((ch & 0x10) == 0x10);
        }

        private void SetLow4Bit(char ch)
        {
            bus.DataBit3.Write((ch & 0x08) == 0x08);
            bus.DataBit2.Write((ch & 0x04) == 0x04);
            bus.DataBit1.Write((ch & 0x02) == 0x02);
            bus.DataBit0.Write((ch & 0x01) == 0x01);
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
    }
}