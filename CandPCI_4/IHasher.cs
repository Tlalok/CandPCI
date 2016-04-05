using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_4
{
    public interface IHasher
    {
        byte[] CalcHash(byte[] data);
    }
}
