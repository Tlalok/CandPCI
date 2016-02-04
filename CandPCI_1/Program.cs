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
            //TestCaesar();
            //TestCardan();

            //TestColumn();

            TestAffine();
        }

        private static void TestCaesar()
        {
            var message = "ЭТОЛАБОРАТОРНАЯРАБОТАПОКИОКИ";
            var key = 3;
            var key2 = 5;
            var encryptedMessage = new CaesarCipher().Encrypt(message, key);
            var decryptedMessage = new CaesarCipher().Decrypt(encryptedMessage, key2);
            Console.WriteLine("Encoded message = {0}", encryptedMessage);
            Console.WriteLine("Decoded message = {0}", decryptedMessage);
        }

        private static void TestColumn()
        {
            //var message = "ЭТОЛАБОРАТОРНАЯРАБОТАПОКИОКИ";
            var message = "ЭТО–_ЛЕКЦИЯ_ПО_АЛГОРИТМАМ_ШИФРОВАНИЯ";
            var key = "КРИПТОГРАФИЯ";
            var key2 = "КОИПТОГРАФИЯ";
            var encryptedMessage = new ColumnarTranspositionCipher().Encrypt(message, key);
            var decryptedMessage = new ColumnarTranspositionCipher().Decrypt(encryptedMessage, key2);
            Console.WriteLine("Encoded message = {0}", encryptedMessage);
            Console.WriteLine("Decoded message = {0}", decryptedMessage);
        }

        private static void TestFence()
        {
            //var message = "CRYPTOGRAPHY";
            //var key = 3;
            var message = "ЛЕКЦИЯ_ПО_АЛГОРИТМАМ_ШИФРО";
            var key = 5;
            var encryptedMessage = new RailFenceCipher().Encrypt(message, key);
            var decryptedMessage = new RailFenceCipher().Decrypt(encryptedMessage, key);
            Console.WriteLine("Encoded message = {0}", encryptedMessage);
            Console.WriteLine("Decoded message = {0}", decryptedMessage);
        }

        private static void TestCardan()
        {
            var message = "КРИПТОГР";
            var key = Key3();
            //var message = "ЭТОЛЕКЦИЯПОКРИПТОГРАФИИ";
            //var key = Key4();
            //var message = "ЭТОЛЕК ЦИЯПОК РИПТОГ РАФИИ1";
            //var message = "ЭТОЛЕКЦИЯПОКРИПТОГРАФИИ1";
            //var key = Key5();
            var encryptedMessage = new CardanGrilleCipher().Encrypt(message, key);
            var decryptedMessage = new CardanGrilleCipher().Decrypt(encryptedMessage, key);
            Console.WriteLine("Encoded message = {0}", encryptedMessage);
            Console.WriteLine("Decoded message = {0}", decryptedMessage);
        }

        private static CardanGrilleKey Key3()
        {
            return new CardanGrilleKey()
            {
                MatrixOrder = 3,
                Positions = new Position[]  
                { 
                    new Position(1, 0),
                    new Position(0, 0)
                }
            };
        }

        private static CardanGrilleKey Key4()
        {
            return new CardanGrilleKey()
            {
                MatrixOrder = 4,
                Positions = new Position[]  
                { 
                    new Position(0, 0),
                    new Position(1, 3),
                    new Position(2, 2),
                    new Position(3, 1)
                }
            };
        }

        private static CardanGrilleKey Key5()
        {
            return new CardanGrilleKey()
            {
                MatrixOrder = 5,
                Positions = new Position[]  
                { 
                    new Position(1, 1),
                    new Position(0, 0),
                    new Position(1, 2),
                    new Position(0, 1),
                    new Position(0, 2),
                    new Position(0, 3)
                }
            };
        }

        private static void TestAffine()
        {
            var message = "ЭТОЛЕКЦИЯПОКРИПТОГРАФИИ";
            var key = 5;
            //var message = "ЭТОЛЕКЦИЯПОКРИПТОГРАФИИ";
            //var key = Key4();
            //var message = "ЭТОЛЕК ЦИЯПОК РИПТОГ РАФИИ1";
            //var message = "ЭТОЛЕКЦИЯПОКРИПТОГРАФИИ1";
            //var key = Key5();
            var encryptedMessage = new AffineCipher().Encrypt(message, key);
            var decryptedMessage = new AffineCipher().Decrypt(encryptedMessage, key);
            Console.WriteLine("Encoded message = {0}", encryptedMessage);
            Console.WriteLine("Decoded message = {0}", decryptedMessage);
        }
    }
}
