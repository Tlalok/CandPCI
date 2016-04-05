using CandPCI_3.PrimalityTesters;
using CandPCI_3.PrimeNumberGenerators;
using CandPCI_4.RSA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_4.DigitalSingature
{
    public class DigitalSignature
    {
        private RsaCryptosystem rsa = new RsaCryptosystem(new PrimeNumberGenerator(new MillerRabinTest()));
        private IHasher hasher;

        public DigitalSignature(IHasher hasher)
        {
            this.hasher = hasher;
        }

        public byte[] Sign(byte[] file, PublicKey key)
        {
            var hash = hasher.CalcHash(file);
            return rsa.Encrypt(hash, key);
        }

        public bool CheckSign(byte[] file, byte[] signature, PrivateKey key)
        {
            var decryptedHash = rsa.Decrypt(signature, key);
            var calculatedHash = hasher.CalcHash(file);
            return Enumerable.SequenceEqual(decryptedHash, calculatedHash);
        }
    }
}
