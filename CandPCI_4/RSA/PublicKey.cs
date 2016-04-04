using System;
using System.Numerics;

namespace CandPCI_4.RSA
{
    [Serializable]
    public class PublicKey
    {
        public BigInteger r { get; set; }
        public BigInteger e { get; set; }
    }
}
