using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_2
{
    class Program
    {
        static void Main(string[] args)
        {


            var des = new SimpleDes();

            //byte text = 0x72;  //  0111 0010
            //short key = 0x282; // 10100 00010 
            //byte expected = 0x77;
            //// Ciphertext:         0111 0111

            byte text = 0xD5; // 1101 0101
            short key = 0x1D1; // 01110 10001
            byte expected = 0x73;
            // Ciphertext: 0111 0011

            //byte text = 0x4C; //   0100 1100
            //short key = 0x3FF; // 11111 11111
            //byte expected = 0x22;
            //// Ciphertext:         0010 0010

            var encryptedText = des.Encrypt(new byte[] { text }, key);
            var decryptedText = des.Decrypt(encryptedText, key);
            Console.WriteLine("Text     = {0}", Convert.ToString(text, 2));
            Console.WriteLine("Key      = {0}", Convert.ToString(key, 2));
            Console.WriteLine("Expected = {0}\n", Convert.ToString(expected, 2));

            Console.WriteLine("encryptedText = {0}", Convert.ToString(encryptedText[0], 2));
            Console.WriteLine("decryptedText = {0}", Convert.ToString(decryptedText[0], 2));
        }
    }
}
