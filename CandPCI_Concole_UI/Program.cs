using CandPCI_3;
using CandPCI_3.PrimalityTesters;
using CandPCI_3.PrimeNumberGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;

namespace CandPCI_Concole_UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourceFile = @"C:\Users\Vladislav_Samodin\Downloads\(1) The Hunger Games.txt";
            var encryptedFile = @"C:\Users\Vladislav_Samodin\Downloads\(1) The Hunger Games111.txt";
            var decryptedFile = @"C:\Users\Vladislav_Samodin\Downloads\(1) The Hunger Games222.txt";

            TestFile(sourceFile, encryptedFile, decryptedFile);

            //RabinCryptosystem rabin = new RabinCryptosystem(new PrimeNumberGenerator(new MillerRabinTest()));
            //BinaryFormatter bf = new BinaryFormatter();

            //var privateKey = (PrivateKey)bf.Deserialize(File.OpenRead(sourceFile + ".private"));
            //var publicKey = (PublicKey)bf.Deserialize(File.OpenRead(sourceFile + ".public"));
            //var encrypted = File.ReadAllBytes(encryptedFile);
            //var decrypted = rabin.Decrypt(encrypted, privateKey, publicKey);

            //File.WriteAllBytes(decryptedFile, decrypted);

            Console.ReadKey();
        }

        private static void TestFile(string sourceFile, string encryptedFile, string decryptedFile)
        {
            RabinCryptosystem rabin = new RabinCryptosystem(new PrimeNumberGenerator(new MillerRabinTest()));
            Stopwatch watch = new Stopwatch();
            var message = File.ReadAllBytes(sourceFile);

            watch.Start();
            var privateKey = rabin.GeneratePrivateKey();
            var publicKey = rabin.GeneratePublicKey(privateKey);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(File.OpenWrite(sourceFile + ".private"), privateKey);
            bf.Serialize(File.OpenWrite(sourceFile + ".public"), publicKey);
            watch.Stop();
            Console.WriteLine("Keys generation time = {0}", watch.ElapsedMilliseconds);

            watch.Reset();
            watch.Start();
            var encrypted = rabin.Encrypt(message, publicKey);
            watch.Stop();
            File.WriteAllBytes(encryptedFile, encrypted);
            Console.WriteLine("Encryption time = {0}", watch.ElapsedMilliseconds);
            encrypted = File.ReadAllBytes(encryptedFile);

            watch.Reset();
            watch.Start();
            var decrypted = rabin.Decrypt(encrypted, privateKey, publicKey);
            watch.Stop();
            Console.WriteLine("Decryption time = {0}", watch.ElapsedMilliseconds);

            File.WriteAllBytes(decryptedFile, decrypted);
        }

        private static void TestMessage()
        {
            RabinCryptosystem rabin = new RabinCryptosystem(new PrimeNumberGenerator(new MillerRabinTest()));
            //36157
            var privateKey = rabin.GeneratePrivateKey();
            var publicKey = rabin.GeneratePublicKey(privateKey);
            var message = Encoding.Unicode.GetBytes(
            @"Конфигурация является такой же важной частью как и код, особенно в крупных проектах. Но часто отношение к ней, как к второсортному артефакту разработки и эксплуатации ПО. Плохо если конфигурация не проходит тот же полный цикл, что и ПО. Про аудит изменений и версионирование забывают, либо проводят не самым подходящим для этого инструментарием.
Я видел много проектов, где конфигурация подкладывается в файловую систему в виде properties/json/xml файлов с непостижимыми уму переоределениями в момент загрузки. И что же на самом деле использует приложение становится ясно только после просмотра лог файлов компонента либо во время отладки.
Скрипт для автоустановки своего Git репозитария можете найти в разделе «Конфигурация в собственном git репозитарии».
При выборе git можно использовать весь тот инструментарий, что используется в разработке. Делать ревью, сравнивать конфигурацию, поддерживать разные ветки конфигурации, ссылаться на теги, пользоваться как визуальными инструментами так и инструментарием в командной строке.
Чтобы воспользоваться всем этим мощным арсеналом нам надо лишь научиться считывать конфигурацию из git репозитария.");
            var encrypted = rabin.Encrypt(message, publicKey);
            var decrypted = rabin.Decrypt(encrypted, privateKey, publicKey);
            Console.WriteLine("Source message = {0}", Encoding.Unicode.GetString(message));
            Console.WriteLine();

            Console.WriteLine("Decrepted message = {0}", Encoding.Unicode.GetString(decrypted));
            Console.WriteLine();
        }

        private static void TestOneByte()
        {
            RabinCryptosystem rabin = new RabinCryptosystem(new PrimeNumberGenerator(new MillerRabinTest()));
             //36157
            var privateKey = new PrivateKey { p = 3163, q = 3167 };
            var publicKey = new PublicKey { n = 10017221, b = 4562931 };
            var message = new byte[] { 61 };

            var encrypted = rabin.Encrypt(message, publicKey);
            var decrypted = rabin.Decrypt(encrypted, privateKey, publicKey);

            Console.Write("encrypted = ");
            for (var i = 0; i < encrypted.Length; i++)
                Console.Write("{0} ", encrypted[i]);
            Console.WriteLine();

            Console.Write("decrypted = ");
            for (var i = 0; i < decrypted.Length; i++)
                Console.Write("{0} ", decrypted[i]);
            Console.WriteLine();

        }
    }
}
