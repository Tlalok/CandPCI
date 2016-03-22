using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_3.PrimalityTesters
{
    public interface IPrimalityTester
    {
        bool IsPrime(BigInteger number);
    }
}
