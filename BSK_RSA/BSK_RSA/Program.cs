// See https://aka.ms/new-console-template for more information

using System.Net.Security;
using System.Numerics;
using System;


namespace BSK_RSA 
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string testencrypt1 = "testencrypt1.txt";
            string testencrypt2 = "testencrypt2.txt";
            string encrypted1 = "encrypted1.txt";
            string encrypted2 = "encrypted2.txt";
            RSAAlgorithm test = new RSAAlgorithm();
            test.Initialize();
            //test.Encrypt(testencrypt1, encrypted1);

            string testdecrypt1 = "encrypted1.txt";  
            string testdecrypt2 = "encrypted2.txt";
            string decrypted1 = "decrypted1.txt";
            string decrypted2 = "decrypted2.txt";

            test.Decrypt(testdecrypt1, decrypted2);

        }

    }
}
