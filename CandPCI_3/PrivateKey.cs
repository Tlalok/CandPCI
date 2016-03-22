using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_3
{
    [Serializable]
    public class PrivateKey
    {
        public BigInteger p { get; set; }
        public BigInteger q { get; set; }
    }
}
