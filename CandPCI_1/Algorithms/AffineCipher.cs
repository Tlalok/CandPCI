using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_1.Algorithms
{
    class AffineCipher
    {
        private char[] alphabet;

        public AffineCipher()
        {
            alphabet = EnglishAlphabet
                .Concat(RussianAlphabet)
                .Concat(PunctuationMarks)
                .ToArray();
        }

        public string Encrypt(string message, int key1, int key2 = 0)
        {
            ValidateKey(key1);

            message = message.ToUpper();
            return new string(message
                .Select(c => EncodeLetter(c, key1, key2))
                .ToArray());
        }

        public string Decrypt(string message, int key1, int key2 = 0)
        {
            ValidateKey(key1);
           
            message = message.ToUpper();
            var inverseKey1 = Inverse(key1, alphabet.Length);
            return new string(message
                .Select(c => DecodeLetter(c, inverseKey1, key2))
                .ToArray());
        }

        private char EncodeLetter(char letter, int key1, int key2 = 0)
        {
            var letterIndex = Array.IndexOf(alphabet, letter);
            var shiftedIndex = letterIndex * key1 + key2;
            var normilizedIndex = RemainderAfterDividing(shiftedIndex, alphabet.Length);
            return alphabet[normilizedIndex];
        }

        private char DecodeLetter(char letter, int inverseKey1, int key2 = 0)
        {
            var letterIndex = Array.IndexOf(alphabet, letter);
            var shiftedIndex = inverseKey1 * (letterIndex - key2);
            var normilizedIndex = RemainderAfterDividing(shiftedIndex, alphabet.Length);
            return alphabet[normilizedIndex];
        }

        private void ValidateKey(int key1)
        {
            var inverseKey1 = Inverse(key1, alphabet.Length);
            if (inverseKey1 == 0)
            {
                throw new ArgumentException("Invalid key1. It must not contain divisor 67");
            }
        }

        private int RemainderAfterDividing(int lhs, int rhs)
        {
            var result = lhs % rhs;
            result = result < 0 ? result + rhs : result;
            return result;
        }

        private char[] EnglishAlphabet
        {
            get
            {
                return Enumerable.Range('A', 26).Select(x => (char)x).ToArray();
            }
        }

        private char[] RussianAlphabet
        {
            get
            {
                return Enumerable.Range('А', 32).Select(x => (char)x).ToArray();
            }
        }

        private char[] PunctuationMarks
        {
            get
            {
                return new char[] { ' ', '_', '.', ',', '!', '?', ':', '-', ';' };
            }
        }

        /* calculates a * x + b * y = gcd(a, b) = d */
        private void ExtendedEuclid(long a, long b, out long x, out long y, out long d)
        {
            long q, r, x1, x2, y1, y2;
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

        private void ExtendedEuclid(int a, int b, out int x, out int y, out int d)
        {
            long longX, longY, longD;
            ExtendedEuclid(a, b, out longX, out longY, out longD);
            x = (int)longX;
            y = (int)longY;
            d = (int)longD;
        }

        /* computes the inverse of a modulo n */
        long Inverse(long a, long n)
        {
            long d, x, y;
            ExtendedEuclid(a, n, out x, out y, out d);
            if (d == 1)
            {
                return x;
            }
            return 0;
        }

        int Inverse(int a, int n)
        {
            return (int)Inverse((long)a, (long)n);
        }

        //int Inverse(int key, int alphabetLength)
        //{
        //    var inverseKey = 1;
        //    var countIterations = alphabetLength - 2;
        //    for (var i = 0; i < countIterations; i++)
        //        inverseKey = (inverseKey * key) % alphabetLength;
        //    return inverseKey;
        //}


    }
}
