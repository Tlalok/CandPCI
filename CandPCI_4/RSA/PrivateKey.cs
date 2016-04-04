using System;
using System.Numerics;

namespace CandPCI_4.RSA
{
    [Serializable]
    public class PrivateKey
    {
        public BigInteger r { get; set; }
        public BigInteger d { get; set; }
    }
}
