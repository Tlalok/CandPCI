using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_1.Algorithms
{
    class FenceEncoder
    {
        public string Encode(string message, int key)
        {
            if (key < 2)
                throw new ArgumentException();
            
            var encodedMessage = String.Empty;
            encodedMessage += EncodeFirstString(message, key);
            for (var i = 1; i < key - 1; i++)
                encodedMessage += EncodeMiddleString(message, key, i);
            encodedMessage += EncodeLastString(message, key);
            return encodedMessage;
        }

        private string EncodeFirstString(string message, int key)
        {
            return EncodeFirstOrLastString(message, key, true);
        }

        private string EncodeLastString(string message, int key)
        {
            return EncodeFirstOrLastString(message, key, false);
        }

        private string EncodeFirstOrLastString(string message, int key, bool first)
        {
            var start = first ? 0 : key - 1;
            var result = new StringBuilder();
            var step = 2 * (key - 1);

            for (var i = start; i < message.Length; i += step)
                result.Append(message[i]);

            return result.ToString();
        }

        private string EncodeMiddleString(string message, int key, int rowIndex)
        {
            var result = new StringBuilder();
            var firstStep = 2 * (key - rowIndex - 1);
            var secondStep = 2 * (key - 1) - firstStep;
            bool first = false;
            Func<int> nextStep = () => (first = !first) ? firstStep : secondStep;

            for (var i = rowIndex; i < message.Length; i += nextStep())
                result.Append(message[i]);

            return result.ToString();
        }

        public string Decode(string message, int key)
        {
            //var decodedMessage = new StringBuilder(message.Length);
            var decodedMessage = new StringBuilder(new string(' ', message.Length));
            var messageEnumerator = message.GetEnumerator();
            messageEnumerator.MoveNext();

            DecodeFirstString(messageEnumerator, key, decodedMessage);
            for (var i = 1; i < key - 1; i++)
                DecodeMiddleString(messageEnumerator, key, decodedMessage, i);
            DecodeLastString(messageEnumerator, key, decodedMessage);
            return decodedMessage.ToString();
        }

        private void DecodeFirstString(CharEnumerator message, int key, StringBuilder result)
        {
            DecodeFirstOrLastString(message, key, result, true);
        }

        private void DecodeLastString(CharEnumerator message, int key, StringBuilder result)
        {
            DecodeFirstOrLastString(message, key, result, false);
        }

        private string DecodeFirstOrLastString(CharEnumerator message, int key, StringBuilder result, bool first)
        {
            var start = first ? 0 : key - 1;
            var step = 2 * (key - 1);

            for (var i = start; i < result.Length; i += step, message.MoveNext())
                result[i] = message.Current;

            return result.ToString();
        }

        private void DecodeMiddleString(CharEnumerator message, int key, StringBuilder result, int rowIndex)
        {
            var firstStep = 2 * (key - rowIndex - 1);
            var secondStep = 2 * (key - 1) - firstStep;
            bool first = false;
            Func<int> nextStep = () => (first = !first) ? firstStep : secondStep;

            for (var i = rowIndex; i < result.Length; i += nextStep(), message.MoveNext())
                result[i] = message.Current;
        }

    }
}
