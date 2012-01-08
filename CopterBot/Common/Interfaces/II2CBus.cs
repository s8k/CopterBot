using System;

namespace CopterBot.Common.Interfaces
{
    public interface II2CBus : IDisposable
    {
        void Write(byte register, byte value);

        byte Read(byte register);

        void Update(byte register, ActionWithByte action);

        byte[] ReadSequence(byte register, int count);
    }
}