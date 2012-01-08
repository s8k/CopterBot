using CopterBot.Common.Interfaces;
using Microsoft.SPOT.Hardware;

namespace CopterBot.Common
{
    public class I2CBus : II2CBus
    {
        private readonly I2CDevice device;
        private readonly int timeout;


        public I2CBus(ushort address, int clockRate, int timeout)
        {
            device = new I2CDevice(new I2CDevice.Configuration(address, clockRate));
            this.timeout = timeout;
        }

        public void Dispose()
        {
            device.Dispose();
        }

        public void Write(byte register, byte value)
        {
            Execute(I2CDevice.CreateWriteTransaction(new [] { register, value }));
        }

        public byte Read(byte register)
        {
            var readBuffer = ReadSequence(register, 1);

            return readBuffer[0];
        }

        public void Update(byte register, ActionWithByte action)
        {
            Write(register, action(Read(register)));
        }

        public byte[] ReadSequence(byte register, int count)
        {
            var readBuffer = new byte[count];

            Execute(
                I2CDevice.CreateWriteTransaction(new[] { register }),
                I2CDevice.CreateReadTransaction(readBuffer));

            return readBuffer;
        }

        private void Execute(params I2CDevice.I2CTransaction[] transactions)
        {
            device.Execute(transactions, timeout);
        }
    }
}