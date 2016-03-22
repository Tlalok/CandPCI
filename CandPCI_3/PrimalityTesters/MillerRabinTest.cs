using CandPCI_3.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_3.PrimalityTesters
{
    public class MillerRabinTest : IPrimalityTester
    {
        public bool IsPrime(BigInteger number, int certainty)
        {
            if (number == 2 || number == 3)
                return true;
            if (number < 2 || number % 2 == 0)
                return false;

            BigInteger d = number - 1;
            int s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s++;
            }

            var upperBoundLength = number.ToByteArray().Length - 1;
            for (int i = 0; i < certainty; i++)
            {
                //var a = BigIntegerHelper.PositiveOddRandom(2, number - 2);
                //var a = BigIntegerHelper.PositiveOddRandom(1, number.ToByteArray().Length * 8 - 2);
                var a = BigIntegerHelper.PositiveOddRandom(1, upperBoundLength);

                BigInteger x = BigInteger.ModPow(a, d, number);
                if (x == 1 || x == number - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, number);
                    if (x == 1)
                        return false;
                    if (x == number - 1)
                        break;
                }

                if (x != number - 1)
                    return false;
            }

            return true;
        }

        public bool IsPrime(BigInteger number)
        {
            var certainty = (int)BigInteger.Log(number, 2);
            return IsPrime(number, certainty);
        }
    }
}
