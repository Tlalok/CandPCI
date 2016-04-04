using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_3.Helpers
{
    public static class BigIntegerHelper
    {
        public static BigInteger PositiveOddRandom(BigInteger lowerBound, BigInteger upperBound)
        {
            byte[] bytes = upperBound.ToByteArray();
            BigInteger result;
            var random = new Random(DateTime.Now.Millisecond);
            do
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= (byte)0x7F; //force sign bit to positive
                bytes[0] = (byte)(bytes[0] | 1);
                result = new BigInteger(bytes);
            } while (result < lowerBound || result >= upperBound);

            return result;
        }

        //public static BigInteger PositiveOddRandom(int lowerBoundExp, int upperBoundExp)
        //{
        //    var random = new Random(DateTime.Now.Millisecond);
        //    byte[] minorPart = BigInteger.Pow(2, lowerBoundExp).ToByteArray();
        //    random.NextBytes(minorPart);
        //    minorPart[0] = (byte)(minorPart[0] | 1);
        //    byte[] majorPart = BigInteger.Pow(2, upperBoundExp - lowerBoundExp).ToByteArray();
        //    do
        //    {
        //        random.NextBytes(majorPart);
        //        majorPart[majorPart.Length - 1] &= (byte)0x7F;
        //        //var result = new BigInteger(majorPart);
        //    } while (majorPart.All(b => b == 0));

        //    return new BigInteger(minorPart.Concat(majorPart).ToArray());
        //}

        public static BigInteger PositiveOddRandom(int lowerBoundExp, int upperBoundExp)
        {
            var random = new Random(DateTime.Now.Millisecond);
            byte[] minorPart = new byte[lowerBoundExp];
            random.NextBytes(minorPart);
            minorPart[0] = (byte)(minorPart[0] | 1);
            byte[] majorPart = new byte[upperBoundExp - lowerBoundExp];
            do
            {
                random.NextBytes(majorPart);
                majorPart[majorPart.Length - 1] &= (byte)0x7F;
            } while (majorPart.All(b => b == 0));

            return new BigInteger(minorPart.Concat(majorPart).ToArray());
        }

        public static BigInteger fast_exp(BigInteger a, BigInteger z, BigInteger n)
        {
            var a1 = a;
            var z1 = z;
            BigInteger x = 1;
            while (z1 != 0)
            {
                while ((z1 % 2) == 0)
                {
                    z1 = z1 / 2;
                    a1 = (a1 * a1) % n;
                }
                z1--;
                x = (x * a1) % n;
            }
            return x;
        }

        public static BigInteger GetInverse(BigInteger number, BigInteger mod)
        {
            BigInteger d, x, y;
            ExtendedEuclid(number, mod, out x, out y, out d);
            if (d == 1)
            {
                return x;
            }
            return 0;
        }

        private static void ExtendedEuclid(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y, out BigInteger d)
        {
            BigInteger q, r, x1, x2, y1, y2;
            if (b == 0)
            {
                d = a;
                x = 1;
                y = 0;
                return;
            }
            x2 = 1;
            x1 = 0;
            y2 = 0;
            y1 = 1;
            while (b > 0)
            {
                q = a / b;
                r = a - q * b;

                x = x2 - q * x1;
                y = y2 - q * y1;

                a = b;
                b = r;

                x2 = x1;
                x1 = x;
                y2 = y1;
                y1 = y;
            }

            d = a;
            x = x2;
            y = y2;
        }
    }
}
