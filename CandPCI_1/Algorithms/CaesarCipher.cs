using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_1.Algorithms
{
    class CaesarCipher
    {
        private char[] alphabet;

        public CaesarCipher()
        {
            alphabet = GetEnglishAlphabet()
                .Concat(GetRussianAlphabet())
                .Concat(GetSymbols())
                .ToArray();
        }

        public string Encode(string message, int key)
        {
            message = message.ToUpper();
            return new string(message
                .Select(c => EncodeLetter(c, key))
                .ToArray());
        }

        public string Decode(string message, int key)
        {
            message = message.ToUpper();
            return new string(message
                .Select(c => DecodeLetter(c, key))
                .ToArray());
        }

        private char EncodeLetter(char letter, int key)
        {
            var letterIndex = Array.IndexOf(alphabet, letter);
            var shiftedIndex = letterIndex + key;
            var normilizedIndex = RemainderAfterDividing(shiftedIndex, alphabet.Length);
            return alphabet[normilizedIndex];
        }

        private char DecodeLetter(char letter, int key)
        {
            var letterIndex = Array.IndexOf(alphabet, letter);
            var shiftedIndex = letterIndex - key;
            var normilizedIndex = RemainderAfterDividing(shiftedIndex, alphabet.Length);
            return alphabet[normilizedIndex];
        }

        private int RemainderAfterDividing(int lhs, int rhs)
        {
            var result = lhs % rhs;
            result = result < 0 ? result + rhs : result;
            return result;
        }

        private char[] GetEnglishAlphabet()
        {
            return Enumerable.Range('A', 26).Select(x => (char)x).ToArray();
        }

        private char[] GetRussianAlphabet()
        {
            return Enumerable.Range('А', 32).Select(x => (char)x).ToArray();
        }

        private char[] GetSymbols()
        {
            return new char[] { ' ', '_', '.', ',', '!', '?', ':', '-' };
        }
    }
}
