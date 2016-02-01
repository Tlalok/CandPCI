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
            ValidateInputData(message, key);

            var encoder = new Ecoder(message, key);
            encoder.EncodeFirstString();
            for (var i = 1; i < key - 1; i++)
                encoder.EncodeMiddleString(i);
            encoder.EncodeLastString();
            return encoder.GetResult();
        }

        public string Decode(string message, int key)
        {
            ValidateInputData(message, key);

            var decoder = new Decoder(message, key);
            decoder.DecodeFirstString();
            for (var i = 1; i < key - 1; i++)
                decoder.DecodeMiddleString(i);
            decoder.DecodeLastString();
            return decoder.GetResult();
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


        private class Ecoder
        {
            private StringBuilder result;

            public string Message { get; private set; }
            public int Key { get; private set; }

            public Ecoder(string message, int key)
            {
                this.Message = message;
                this.Key = key;
                result = new StringBuilder(message.Length);
            }

            public void EncodeFirstString()
            {
                EncodeFirstOrLastString(true);
            }

            public void EncodeLastString()
            {
                EncodeFirstOrLastString(false);
            }

            private void EncodeFirstOrLastString(bool first)
            {
                var start = first ? 0 : Key - 1;
                var step = 2 * (Key - 1);

                for (var i = start; i < Message.Length; i += step)
                    result.Append(Message[i]);
            }

            public void EncodeMiddleString(int rowIndex)
            {
                var firstStep = 2 * (Key - rowIndex - 1);
                var secondStep = 2 * (Key - 1) - firstStep;
                //firstStep = firstStep == 0 ? secondStep : firstStep;
                //secondStep = secondStep == 0 ? firstStep : secondStep;
                bool first = false;
                Func<int> nextStep = () => (first = !first) ? firstStep : secondStep;

                for (var i = rowIndex; i < Message.Length; i += nextStep())
                    result.Append(Message[i]);
            }

            public string GetResult()
            {
                return result.ToString();
            }

            public void Clear()
            {
                result = new StringBuilder(Message.Length);
            }
        }

        private class Decoder
        {
            private StringBuilder result;
            private CharEnumerator messageEnumerator;

            public string Message { get; private set; }
            public int Key { get; private set; }

            public Decoder(string message, int key)
            {
                this.Message = message;
                this.Key = key;

                result = new StringBuilder(new string(' ', message.Length));
                messageEnumerator = message.GetEnumerator();
                messageEnumerator.MoveNext();
            }

            public void DecodeFirstString()
            {
                DecodeFirstOrLastString(true);
            }

            public void DecodeLastString()
            {
                DecodeFirstOrLastString(false);
            }

            private void DecodeFirstOrLastString(bool first)
            {
                var start = first ? 0 : Key - 1;
                var step = 2 * (Key - 1);

                for (var i = start; i < result.Length; i += step, messageEnumerator.MoveNext())
                    result[i] = messageEnumerator.Current;
            }

            public void DecodeMiddleString(int rowIndex)
            {
                var firstStep = 2 * (Key - rowIndex - 1);
                var secondStep = 2 * (Key - 1) - firstStep;
                bool first = false;
                Func<int> nextStep = () => (first = !first) ? firstStep : secondStep;

                for (var i = rowIndex; i < result.Length; i += nextStep(), messageEnumerator.MoveNext())
                    result[i] = messageEnumerator.Current;
            }

            public string GetResult()
            {
                return result.ToString();
            }

            public void Clear()
            {
                result = new StringBuilder(new string(' ', Message.Length));
                messageEnumerator = Message.GetEnumerator();
                messageEnumerator.MoveNext();
            }
        }

    }
}
