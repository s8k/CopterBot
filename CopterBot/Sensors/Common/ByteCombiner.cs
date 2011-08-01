using System;

namespace CopterBot.Sensors.Common
{
    public class ByteCombiner
    {
        public static Int16 TwoBytes(byte[] array, int skip = 0)
        {
            return (Int16)Combine(2, array, skip);
        }

        public static Int32 ThreeBytes(byte[] array, int skip = 0)
        {
            return Combine(3, array, skip);
        }

        private static Int32 Combine(int count, byte[] array, int skip)
        {
            CheckArraySize(count, array, skip);

            var result = 0;
            for (var i = 0; i < count; i++)
            {
                result |= array[skip + i] << (8 * (count - i - 1));
            }

            return result;
        }

        private static void CheckArraySize(int count, byte[] array, int skip)
        {
            if (array.Length - skip < count)
            {
                throw new ArgumentException(string.Concat("There is no ", count, " bytes to combine."));
            }
        }
    }
}