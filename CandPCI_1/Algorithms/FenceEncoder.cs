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
            encodedMessage += GetFirstString(message, key);
            for (var i = 1; i < key - 1; i++)
                encodedMessage += GetMiddleString(message, key, i);
            encodedMessage += GetLastString(message, key);
            return encodedMessage;
        }

        private string GetFirstString(string message, int key)
        {
            return GetFirstOrLastString(message, key, true);
        }

        private string GetLastString(string message, int key)
        {
            return GetFirstOrLastString(message, key, false);
        }

        private string GetFirstOrLastString(string message, int key, bool first)
        {
            var start = first ? 0 : key - 1;
            var result = new StringBuilder();
            var step = 2 * (key - 1);

            for (var i = start; i < message.Length; i += step)
                result.Append(message[i]);

            return result.ToString();
        }

        private string GetMiddleString(string message, int key, int rowIndex)
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


        //private char[,] GenerateMatrix(string message, int key)
        //{
        //    var columnCount = message.Length / key;
        //    var result = ;
        //    for (int j = 0; j < columnCount; j++)
        //    {
        //        var columnEven = j % 2 == 0;
        //        var increment = columnEven ? 1 : -1;
        //        var startRow = columnEven ? 0 : key - 1;
        //        Predicate<int> condition;
        //        if (columnEven)
        //            condition = i => i < key;
        //        else
        //            condition = i => i >= 0;
        //        for (var i = startRow; condition(i); i += increment)
        //            result += 
        //    }
        //}

        //public string Decode(string message, int key)
        //{

        //}

    }
}
