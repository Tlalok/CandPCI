using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_1.Algorithms
{
    class ColumnarTranspositionCipher
    {
        private char[] alphabet;

        public ColumnarTranspositionCipher()
        {
            alphabet = EnglishAlphabet
                .Concat(RussianAlphabet)
                .Concat(PunctuationMarks)
                .ToArray();
        }

        public string Encrypt(string message, string key)
        {
            message = message.ToUpper();
            return new Encrypter(alphabet).Encrypt(message, key);
        }

        public string Decrypt(string message, string key)
        {
            message = message.ToUpper();
            return new Decrypter(alphabet).Decrypt(message, key);
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
                return new char[] { ' ', '_', '.', ',', '!', '?', ':', '-' };
            }
        }

        private abstract class ColumnTranspositionTravaller
        {
            private char[] alphabet;

            public ColumnTranspositionTravaller(char[] alphabet)
            {
                this.alphabet = new char[alphabet.Length];
                Array.Copy(alphabet, this.alphabet, alphabet.Length);
            }

            protected void Travel(string message, string key)
            {
                message = message.ToUpper();

                foreach (var letter in alphabet)
                {
                    var position = key.IndexOf(letter);
                    while (position != -1)
                    {
                        for (var i = position; i < message.Length; i += key.Length)
                        {
                            ElementAction(i, message);
                        }
                        position = key.IndexOf(letter, position + 1);
                    }
                }
            }

            protected abstract void ElementAction(int index, string message);
        }

        private class Encrypter : ColumnTranspositionTravaller
        {
            private StringBuilder encodedMessage;
            private int currentPosition;

            public Encrypter(char[] alphabet) : base(alphabet) { }

            public string Encrypt(string message, string key)
            {
                encodedMessage = new StringBuilder(new string(' ', message.Length));
                currentPosition = 0;
                Travel(message, key);
                return encodedMessage.ToString();
            }

            protected override void ElementAction(int index, string message)
            {
                encodedMessage[currentPosition++] = message[index];
            }
        }

        private class Decrypter : ColumnTranspositionTravaller
        {
            private StringBuilder decodedMessage;
            private int currentPosition;

            public Decrypter(char[] alphabet) : base(alphabet) { }

            public string Decrypt(string message, string key)
            {
                decodedMessage = new StringBuilder(new string(' ', message.Length));
                currentPosition = 0;
                Travel(message, key);
                return decodedMessage.ToString();
            }

            protected override void ElementAction(int index, string message)
            {
                decodedMessage[index] = message[currentPosition++];
            }
        }
    }
}
