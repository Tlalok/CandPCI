using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using CandPCI_3.Helpers;
using CandPCI_3.PrimeNumberGenerators;

namespace CandPCI_4.RSA
{
    public class RsaCryptosystem
    {
        private IPrimeNumberGenerator generator;

        public RsaCryptosystem(IPrimeNumberGenerator generator)
        {
            this.generator = generator;
        }

        public RsaKeyContainer GenerateKeys()
        {
            var p = generator.GetPrimeNumber(64, 64);
            var q = generator.GetPrimeNumber(64, 64, n => n != p);
            var r = q * p;
            var phi = (q - 1) * (p - 1);
            var e = generator.GetPrimeNumber(r.ToByteArray().Length - 2, r.ToByteArray().Length - 2, n => n < phi);
            var d = BigIntegerHelper.GetInverse(e, phi);

            return new RsaKeyContainer
            {
                privateKey = new PrivateKey
                {
                    r = r,
                    d = d
                },
                publicKey = new PublicKey
                {
                    r = r,
                    e = e
                }
            };
        }

        public byte[] Encrypt(byte[] message, PublicKey key)
        {
            var keySize = key.r.ToByteArray().Length;
            var encryptedBlockSize = keySize;
            var sourceBlockSize = keySize - 2;

            var numberBlocks = message.Length / sourceBlockSize;
            numberBlocks += message.Length % sourceBlockSize != 0 ? 1 : 0;

            var result = new byte[numberBlocks * encryptedBlockSize];
            Parallel.For(0, numberBlocks, i =>
            //for (var i = 0; i < numberBlocks; i++)
            {
                var part = new byte[encryptedBlockSize];
                var blockLength = message.Length - i * sourceBlockSize < sourceBlockSize ? message.Length - i * sourceBlockSize : sourceBlockSize;
                Array.Copy(message, i * sourceBlockSize, part, 0, blockLength);
                part[part.Length - 2] = 0x00;
                part[part.Length - 1] = 0x00;

                var mi = new BigInteger(part);
                var Mi = BigInteger.ModPow(mi, key.e, key.r);
                var MiBytes = Mi.ToByteArray();
                //while (MiBytes.Length < encryptedBlockSize)
                //    MiBytes = MiBytes.Concat(new byte[] { 0 }).ToArray();
                var MiLength = MiBytes.Length;
                if (MiLength < encryptedBlockSize)
                    MiBytes = MiBytes.Concat(new byte[encryptedBlockSize - MiLength]).ToArray();
                Array.Copy(MiBytes, 0, result, i * encryptedBlockSize, encryptedBlockSize);
            });
            return result;
        }

        public byte[] Decrypt(byte[] message, PrivateKey key)
        {
            var keySize = key.r.ToByteArray().Length;
            var encryptedBlockSize = keySize;
            var sourceBlockSize = keySize - 2;

            var numberBlocks = message.Length / encryptedBlockSize;
            if (message.Length % encryptedBlockSize != 0)
                throw new ArgumentException("Wrong size of encrypted message");

            var decryptedMessage = new byte[numberBlocks * sourceBlockSize];

            Parallel.For(0, numberBlocks - 1, (i) =>
            //for (var i = 0; i < numberBlocks; i++)
            {
                var part = new byte[encryptedBlockSize];

                Array.Copy(message, i * encryptedBlockSize, part, 0, encryptedBlockSize);
                var Mi = new BigInteger(part);
                var mi = BigInteger.ModPow(Mi, key.d, key.r);
                //var miBytes = mi.ToByteArray();
                //var
                //if ()
                Array.Copy(mi.ToByteArray(), 0, decryptedMessage, i * sourceBlockSize, sourceBlockSize);
            });

            var lastPart = new byte[encryptedBlockSize];
            Array.Copy(message, (numberBlocks - 1) * encryptedBlockSize, lastPart, 0, encryptedBlockSize);
            var lMi = new BigInteger(lastPart);
            var lmi = BigInteger.ModPow(lMi, key.d, key.r);
            var lmiBytes = lmi.ToByteArray();
            var countBytes = lmiBytes.Length;
            Array.Copy(lmi.ToByteArray(), 0, decryptedMessage, (numberBlocks - 1) * sourceBlockSize, countBytes);
            var realSize = sourceBlockSize*(numberBlocks - 1) + countBytes;
            var trimMessage = new byte[realSize];
            Array.Copy(decryptedMessage, 0, trimMessage, 0, realSize);

            return trimMessage;
        }
    }
}
