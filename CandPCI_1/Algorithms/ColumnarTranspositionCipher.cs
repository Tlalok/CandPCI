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
            alphabet = GetEnglishAlphabet()
                .Concat(GetRussianAlphabet())
                .Concat(GetSymbols())
                .ToArray();
        }

        public string Encode(string message, string key)
        {
            message = message.ToUpper();
            return new Encoder(alphabet).Encode(message, key);
        }

        public string Decode(string message, string key)
        {
            message = message.ToUpper();
            return new Decoder(alphabet).Decode(message, key);
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

        private class Encoder : ColumnTranspositionTravaller
        {
            private StringBuilder encodedMessage;
            private int currentPosition;

            public Encoder(char[] alphabet) : base(alphabet) { }

            public string Encode(string message, string key)
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

        private class Decoder : ColumnTranspositionTravaller
        {
            private StringBuilder decodedMessage;
            private int currentPosition;

            public Decoder(char[] alphabet) : base(alphabet) { }

            public string Decode(string message, string key)
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
