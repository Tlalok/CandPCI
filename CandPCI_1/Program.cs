using CandPCI_1.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_1
{
    class Program
    {
        static void Main(string[] args)
        {

            TestColumn();

        }

        private static void TestColumn()
        {
            var message = "ЭТОЛАБОРАТОРНАЯРАБОТАПОКИОКИ";
            var key = "КРИПТОГРАФИЯ";
            //var message = "ЛЕКЦИЯ_ПО_АЛГОРИТМАМ_ШИФРО";
            //var key = 5;
            var encodedMessage = new ColumnEncoder().Encode(message, key);
            var decodedMessage = new ColumnEncoder().Decode(encodedMessage, key);
            Console.WriteLine("Encoded message = {0}", encodedMessage);
            Console.WriteLine("Decoded message = {0}", decodedMessage);
        }

        private static void TestFence()
        {
            var message = "CRYPTOGRAPHY";
            var key = 3;
            //var message = "ЛЕКЦИЯ_ПО_АЛГОРИТМАМ_ШИФРО";
            //var key = 5;
            var encodedMessage = new FenceEncoder().Encode(message, key);
            var decodedMessage = new FenceEncoder().Decode(encodedMessage, key);
            Console.WriteLine("Encoded message = {0}", encodedMessage);
            Console.WriteLine("Decoded message = {0}", decodedMessage);
        }
    }
}
