using CandPCI_3.Helpers;
using CandPCI_3.PrimeNumberGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_3
{
    public class RabinCryptosystem
    {
        private readonly byte[] preffix = new byte[] { 0x8D, 0x21, 0xDF, 0xEC};
        //private readonly byte[] preffix = new byte[] { 0x8D };

        private IPrimeNumberGenerator generator;

        public RabinCryptosystem(IPrimeNumberGenerator generator)
        {
            this.generator = generator;
        }

        public byte[] Encrypt(byte[] message, PublicKey key)
        {
            var keySize = key.n.ToByteArray().Length;
            var encryptedBlockSize = keySize;
            var sourceBlockSize = keySize - preffix.Length - 2;

            var numberBlocks = message.Length / sourceBlockSize;
            numberBlocks += message.Length % sourceBlockSize != 0 ? 1 : 0;

            var result = new byte[numberBlocks * encryptedBlockSize];
            Parallel.For(0, numberBlocks, (i) =>
            //for (var i = 0; i < numberBlocks; i++)
            {
                var part = new byte[encryptedBlockSize];
                var blockLength = message.Length - i * sourceBlockSize < sourceBlockSize ? message.Length - i * sourceBlockSize : sourceBlockSize;
                Array.Copy(message, i * sourceBlockSize, part, 0, blockLength);
                Array.Copy(preffix, 0, part, sourceBlockSize, preffix.Length);
                part[part.Length - 2] = 0x00;
                part[part.Length - 1] = 0x00;

                var mi = new BigInteger(part);
                var Mi = (mi * (mi + key.b)) % key.n;
                var MiBytes = Mi.ToByteArray();
                while (MiBytes.Length < encryptedBlockSize)
                    MiBytes = MiBytes.Concat(new byte[] { 0 }).ToArray();
                Array.Copy(MiBytes, 0, result, i * encryptedBlockSize, encryptedBlockSize);
            });
            return result;
        }

        public byte[] Decrypt(byte[] message, PrivateKey privateKey, PublicKey publicKey)
        {
            var keySize = publicKey.n.ToByteArray().Length;
            var encryptedBlockSize = keySize;
            var sourceBlockSize = keySize - preffix.Length - 2;

            var numberBlocks = message.Length / encryptedBlockSize;
            if (message.Length % encryptedBlockSize != 0)
                throw new ArgumentException("Wrong size of encrypted message");

            var decryptedMessage = new byte[numberBlocks * sourceBlockSize];

            Parallel.For(0, numberBlocks, (i) =>
            //for (var i = 0; i < numberBlocks; i++)
            {
                var part = new byte[encryptedBlockSize];

                Array.Copy(message, i * encryptedBlockSize, part, 0, encryptedBlockSize);
                var Mi = new BigInteger(part);
                var D = (publicKey.b * publicKey.b + 4 * Mi) % publicKey.n;
                //var s = BigInteger.ModPow(D, (privateKey.p + 1) / 4, privateKey.p);
                //var r = BigInteger.ModPow(D, (privateKey.q + 1) / 4, privateKey.q);
                var s = BigIntegerHelper.fast_exp(D, (privateKey.p + 1) / 4, privateKey.p);
                var r = BigIntegerHelper.fast_exp(D, (privateKey.q + 1) / 4, privateKey.q);
                BigInteger yp, yq;
                BigInteger d = 1;
                ExtendedEuclid(privateKey.p, privateKey.q, out yp, out yq, out d);
                var roots = new BigInteger[4];

                roots[0] = BigInteger.Abs(yp * privateKey.p * r + yq * privateKey.q * s);
                roots[1] = (-roots[0]) % publicKey.n + publicKey.n;
                roots[0] = roots[0] % publicKey.n;

                roots[2] = BigInteger.Abs(yp * privateKey.p * r - yq * privateKey.q * s);
                roots[3] = (-roots[2]) % publicKey.n + publicKey.n;
                roots[2] = roots[2] % publicKey.n;

                for (var j = 0; j < 4; j++)
                {
                    roots[j] = ((-publicKey.b + roots[j]) / 2) % publicKey.n;
                    roots[j] = roots[j] < 0 ? roots[j] + publicKey.n : roots[j];
                }

                var rightRoot = roots.First(num => num.ToByteArray().Skip(sourceBlockSize).Take(preffix.Length).SequenceEqual(preffix));
                Array.Copy(rightRoot.ToByteArray(), 0, decryptedMessage, i * sourceBlockSize, sourceBlockSize);
            });

            return decryptedMessage;
        }

        private void ExtendedEuclid(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y, out BigInteger d)
        {
            BigInteger q, r, x1, x2, y1, y2;
            if (b == 0)
            {
                d = a;
                x = 1;
                y = 0;
                return;
            }
            x2 = 1;
            x1 = 0;
            y2 = 0;
            y1 = 1;
            while (b > 0)
            {
                q = a / b;
                r = a - q * b;

                x = x2 - q * x1;
                y = y2 - q * y1;

                a = b;
                b = r;

                x2 = x1;
                x1 = x;
                y2 = y1;
                y1 = y;
            }

            d = a;
            x = x2;
            y = y2;
        }

        public PublicKey GeneratePublicKey(PrivateKey key)
        {
            var n = key.p * key.q;
            var b = BigIntegerHelper.PositiveOddRandom(n / 1000000, n);
            return new PublicKey
            {
                n = n,
                b = b
            };
        }

        public PrivateKey GeneratePrivateKey()
        {
            //var lowerBound = BigInteger.Pow(10, 98);
            //var upperBound = BigInteger.Pow(10, 100);

            //var lowerBound = BigInteger.Pow(10, 295);
            //var upperBound = BigInteger.Pow(10, 300);

            //var p = generator.GetPrimeNumber(lowerBound, upperBound, n => n % 4 == 3);
            //var p = generator.GetPrimeNumber(333, 340, n => n % 4 == 3);
            var p = generator.GetPrimeNumber(42, 43, n => n % 4 == 3);

            //var q = generator.GetPrimeNumber(lowerBound, upperBound, n => n % 4 == 3 && n != p);
            //var q = generator.GetPrimeNumber(333, 340, n => n % 4 == 3 && n != p);
            var q = generator.GetPrimeNumber(42, 43, n => n % 4 == 3 && n != p);

            return new PrivateKey
            {
                p = p,
                q = q
            };
        }
    }
}
