using System;

namespace CopterBot.Sensors.Common
{
    public static class ByteCombiner
    {
        public static short TwoLsbFirst(byte[] array, int skip = 0)
        {
            return (short)Combine(2, true, array, skip);
        }
        public static short TwoMsbFirst(byte[] array, int skip = 0)
        {
            return (short)Combine(2, false, array, skip);
        }

        public static int ThreeMsbFirst(byte[] array, int skip = 0)
        {
            return Combine(3, false, array, skip);
        }

        private static int Combine(int count, bool fromLsb, byte[] array, int skip)
        {
            CheckArraySize(count, array, skip);

            var result = 0;
            for (var i = 0; i < count; i++)
            {
                var shift = fromLsb
                                ? i
                                : count - i - 1;

                result |= array[skip + i] << (8 * shift);
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