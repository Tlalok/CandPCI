using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_4
{
    public class Hasher : IHasher
    {
        private BigInteger mod;
        private const int hashLength = 128;

        private readonly BigInteger p = BigInteger.Parse("6618413872461146665294315528417462439731888059980036606512760130348022962009579609085265076742955718943712428917198761555856426140048919002147437235667181");
        private readonly BigInteger q = BigInteger.Parse("6142880917252876962407352390074216025314731019854315085080897409298671219551651818040352387533397517603969051950446942421379907249589924957479622946957631");

        //public Hasher(BigInteger mod)
        //{
        //    this.mod = mod;
        //}

        public Hasher()
        {
            this.mod = p * q;
        }

        public byte[] CalcHash(byte[] data)
        {
            BigInteger hash = 0;
            var countIteration = data.Length / hashLength;
            for (int i = 0; i < countIteration; i++)
            {
                var currentBlock = new byte[hashLength];
                Array.Copy(data, i * hashLength, currentBlock, 0, hashLength);
                var currentNumber = new BigInteger(currentBlock.Concat(new byte[1] { 0 }).ToArray());
                hash = BigInteger.ModPow(hash + currentNumber, 2, mod);
            }
            var lastBytesCount = data.Length % hashLength;
            if (lastBytesCount != 0)
            {
                var lastBlock = new byte[hashLength];
                Array.Copy(data, countIteration * hashLength, lastBlock, 0, lastBytesCount);
                var lastNumber = new BigInteger(lastBlock.Concat(new byte[1] { 0 }).ToArray());
                hash = BigInteger.ModPow(hash + lastNumber, 2, mod);
            }
            var result = hash.ToByteArray().Take(hashLength).ToArray();
            if (result.Length < hashLength)
                result = result.Concat(new byte[hashLength - result.Length]).ToArray();
            return result;
        }
    }
}
