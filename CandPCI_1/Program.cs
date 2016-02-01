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
            //var message = "CRYPTOGRAPHY";
            //var key = 3;
            var message = "ЛЕКЦИЯ_ПО_АЛГОРИТМАМ_ШИФРО";
            var key = 5;
            Console.WriteLine(new FenceEncoder().Encode(message, key));
        }
    }
}
