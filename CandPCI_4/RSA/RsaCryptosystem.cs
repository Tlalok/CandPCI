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

        public const int keyLength = 32;

        public RsaCryptosystem(IPrimeNumberGenerator generator)
        {
            this.generator = generator;
        }

        private PrivateComponents GeneratePrivateComponents()
        {
            var p = generator.GetPrimeNumber(keyLength, keyLength);
            var q = generator.GetPrimeNumber(keyLength, keyLength, n => n != p);
            return new PrivateComponents
            {
                p = p,
                q = q
            };
        }

        private PrivateKey GeneratePrivateKey(PrivateComponents privateComponents, PublicKey publicKey)
        {
            var phi = (privateComponents.q - 1) * (privateComponents.p - 1);
            var d = BigIntegerHelper.GetInverse(publicKey.e, phi);
            return new PrivateKey
            {
                d = d,
                r = publicKey.r
            };
        }

        private PublicKey GeneratePublicKey(PrivateComponents key)
        {
            var r = key.q * key.p;
            var phi = (key.q - 1) * (key.p - 1);
            var rLength = r.ToByteArray().Length;
            var e = generator.GetPrimeNumber(rLength - 2, rLength - 2, n => n < phi);
            return new PublicKey
            {
                e = e,
                r = r
            };
        }

        public RsaKeyContainer GenerateKeys()
        {
            var privateComponents = GeneratePrivateComponents();
            var publicKey = GeneratePublicKey(privateComponents);
            var privatekey = GeneratePrivateKey(privateComponents, publicKey);
            return new RsaKeyContainer
            {
                privateComponents = privateComponents,
                privateKey = privatekey,
                publicKey = publicKey
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
                var MiLength = MiBytes.Length;
                if (MiLength < encryptedBlockSize)
                    MiBytes = MiBytes.Concat(new byte[encryptedBlockSize - MiLength]).ToArray();
                Array.Copy(MiBytes, 0, result, i * encryptedBlockSize, encryptedBlockSize);
            });
            return result;
        }

        public byte[] Decrypt(byte[] message, PrivateComponents privateComponents, PublicKey publicKey)
        {
            var phi = (privateComponents.q - 1) * (privateComponents.p - 1);
            var d = BigIntegerHelper.GetInverse(publicKey.e, phi);

            return Decrypt(message, new PrivateKey { d = d, r = publicKey.r });

            //var keySize = publicKey.r.ToByteArray().Length;
            //var encryptedBlockSize = keySize;
            //var sourceBlockSize = keySize - 2;

            //var numberBlocks = message.Length / encryptedBlockSize;
            //if (message.Length % encryptedBlockSize != 0)
            //    throw new ArgumentException("Wrong size of encrypted message");

            //var decryptedMessage = new byte[numberBlocks * sourceBlockSize];

            //Parallel.For(0, numberBlocks - 1, (i) =>
            ////for (var i = 0; i < numberBlocks; i++)
            //{
            //    var part = new byte[encryptedBlockSize];

            //    Array.Copy(message, i * encryptedBlockSize, part, 0, encryptedBlockSize);
            //    var Mi = new BigInteger(part);
            //    var mi = BigInteger.ModPow(Mi, d, publicKey.r);
            //    Array.Copy(mi.ToByteArray(), 0, decryptedMessage, i * sourceBlockSize, sourceBlockSize);
            //});

            //var lastPart = new byte[encryptedBlockSize];
            //Array.Copy(message, (numberBlocks - 1) * encryptedBlockSize, lastPart, 0, encryptedBlockSize);
            //var lMi = new BigInteger(lastPart);
            //var lmi = BigInteger.ModPow(lMi, d, publicKey.r);
            //var lmiBytes = lmi.ToByteArray();
            //var countBytes = lmiBytes.Length;
            //Array.Copy(lmi.ToByteArray(), 0, decryptedMessage, (numberBlocks - 1) * sourceBlockSize, countBytes);

            //var realSize = sourceBlockSize * (numberBlocks - 1) + countBytes;
            //var trimMessage = new byte[realSize];
            //Array.Copy(decryptedMessage, 0, trimMessage, 0, realSize);

            //return trimMessage;
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
                Array.Copy(mi.ToByteArray(), 0, decryptedMessage, i * sourceBlockSize, sourceBlockSize);
            });

            var lastPart = new byte[encryptedBlockSize];
            Array.Copy(message, (numberBlocks - 1) * encryptedBlockSize, lastPart, 0, encryptedBlockSize);
            var lMi = new BigInteger(lastPart);
            var lmi = BigInteger.ModPow(lMi, key.d, key.r);
            var lmiBytes = lmi.ToByteArray();
            var countBytes = lmiBytes.Length;
            Array.Copy(lmi.ToByteArray(), 0, decryptedMessage, (numberBlocks - 1) * sourceBlockSize, countBytes);

            var realSize = sourceBlockSize * (numberBlocks - 1) + countBytes;
            var trimMessage = new byte[realSize];
            Array.Copy(decryptedMessage, 0, trimMessage, 0, realSize);

            return trimMessage;
        }
    }
}
