using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_3.PrimeNumberGenerators
{
    public interface IPrimeNumberGenerator
    {
        BigInteger GetPrimeNumber(BigInteger lowerBound, BigInteger upperBound);

        BigInteger GetPrimeNumber(BigInteger lowerBound, BigInteger upperBound, Func<BigInteger, bool> predicate);

        BigInteger GetPrimeNumber(int lowerBoundExp, int upperBoundExp);

        BigInteger GetPrimeNumber(int lowerBoundExp, int upperBoundExp, Func<BigInteger, bool> predicate);
    }
}
