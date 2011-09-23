using System;

namespace CopterBot.Visualization
{
    public interface ILcd : IDisposable
    {
        void Init();

        void Print(string text);

        void Print1Line(string text);

        void Print2Line(string text);
    }
}