using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CandPCI_1.Algorithms
{
    class RailFenceCipher
    {
        public string Encrypt(string message, int key)
        {
            ValidateInputData(message, key);
            return new Encrypter().Encrypt(message, key);
        }

        public string Decrypt(string message, int key)
        {
            ValidateInputData(message, key);
            return new Decrypter().Decrypt(message, key);
        }

        private void ValidateInputData(string message, int key)
        {
            if (key >= message.Length)
            {
                throw new ArgumentException("Ключ слишком велик для данного текста");
            }

            if (key < 2)
            {
                throw new ArgumentException("Ключ - натуральное число, большее, чем 1");
            }
        }

        private abstract class RailFenceTraveller
        {
            protected void Travel(string message, int key)
            {
                for (var i = 0; i < key; i++)
                {
                    var firstStep = 2 * (key - i - 1);
                    var secondStep = 2 * i;
                    firstStep = firstStep == 0 ? secondStep : firstStep;
                    secondStep = secondStep == 0 ? firstStep : secondStep;
                    bool first = false;
                    Func<int> nextStep = () => (first = !first) ? firstStep : secondStep;

                    for (var j = i; j < message.Length; j += nextStep())
                        ElementAction(j, message);
                }
            }

            protected abstract void ElementAction(int index, string message);
        }

        private class Encrypter : RailFenceTraveller
        {
            private StringBuilder encodedMessage;
            private int currentPosition;

            public string Encrypt(string message, int key)
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

        private class Decrypter : RailFenceTraveller
        {
            private StringBuilder decodedMessage;
            private int currentPosition;

            public string Decrypt(string message, int key)
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
