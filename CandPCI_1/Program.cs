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
            //TestFence();
            TestCaesar();

            //TestColumn();

        }

        private static void TestCaesar()
        {
            var message = "ЭТОЛАБОРАТОРНАЯРАБОТАПОКИОКИ";
            var key = 3;
            var key2 = 5;
            var encodedMessage = new CaesarCipher().Encrypt(message, key);
            var decodedMessage = new CaesarCipher().Decrypt(encodedMessage, key2);
            Console.WriteLine("Encoded message = {0}", encodedMessage);
            Console.WriteLine("Decoded message = {0}", decodedMessage);
        }

        private static void TestColumn()
        {
            //var message = "ЭТОЛАБОРАТОРНАЯРАБОТАПОКИОКИ";
            var message = "ЭТО–_ЛЕКЦИЯ_ПО_АЛГОРИТМАМ_ШИФРОВАНИЯ";
            var key = "КРИПТОГРАФИЯ";
            var key2 = "КОИПТОГРАФИЯ";
            var encodedMessage = new ColumnarTranspositionCipher().Encrypt(message, key);
            var decodedMessage = new ColumnarTranspositionCipher().Decrypt(encodedMessage, key2);
            Console.WriteLine("Encoded message = {0}", encodedMessage);
            Console.WriteLine("Decoded message = {0}", decodedMessage);
        }

        private static void TestFence()
        {
            //var message = "CRYPTOGRAPHY";
            //var key = 3;
            var message = "ЛЕКЦИЯ_ПО_АЛГОРИТМАМ_ШИФРО";
            var key = 5;
            var encodedMessage = new RailFenceCipher().Encrypt(message, key);
            var decodedMessage = new RailFenceCipher().Decrypt(encodedMessage, key);
            Console.WriteLine("Encoded message = {0}", encodedMessage);
            Console.WriteLine("Decoded message = {0}", decodedMessage);
        }
    }
}
