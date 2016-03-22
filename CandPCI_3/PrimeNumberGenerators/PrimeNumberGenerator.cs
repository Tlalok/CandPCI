using CandPCI_3.Helpers;
using CandPCI_3.PrimalityTesters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_3.PrimeNumberGenerators
{
    public class PrimeNumberGenerator : IPrimeNumberGenerator
    {
        private IPrimalityTester tester;

        public PrimeNumberGenerator(IPrimalityTester tester)
        {
            this.tester = tester;
        }

        public BigInteger GetPrimeNumber(BigInteger lowerBound, BigInteger upperBound)
        {
            BigInteger result;
            do
            {
                result = BigIntegerHelper.PositiveOddRandom(lowerBound, upperBound);
            }
            while (!tester.IsPrime(result));
            return result;
        }

        public BigInteger GetPrimeNumber(BigInteger lowerBound, BigInteger upperBound, Func<BigInteger, bool> predicate)
        {
            BigInteger result;
            do
            {
                result = BigIntegerHelper.PositiveOddRandom(lowerBound, upperBound);
            }
            while (!predicate(result) || !tester.IsPrime(result));
            return result;
        }
    }
}
