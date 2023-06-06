using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Net.Security;
using System.Linq.Expressions;

namespace BSK_RSA
{
    public class RSAAlgorithm
    {
        BigInteger p;
        BigInteger q;
        BigInteger n;
        BigInteger phi;
        BigInteger encryptionKey;
        BigInteger decryptionKey;
        BigInteger[] encrypted;
        string decrypted;
        string x;


        public void Decrypt(string pathin, string pathout) 
        {
            try
            {


                string[] encryptedValues = File.ReadAllLines(pathin);


                StringBuilder decryptedText = new StringBuilder();

                foreach (string encryptedValue in encryptedValues)
                {
                    BigInteger encryptedNumber = BigInteger.Parse(encryptedValue);

                    // Deszyfrowanie: message = ciphertext^d mod n
                    BigInteger decryptedNumber = BigInteger.ModPow(encryptedNumber, decryptionKey, n);
                    byte decryptedByte = (byte)decryptedNumber;

                    decryptedText.Append((char)decryptedByte);
                }

                //byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
               
                using (StreamWriter sw = new StreamWriter(File.Open(pathout, FileMode.Create)))
                {
                  
                        sw.WriteLine(decryptedText.ToString());
                };
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd podczas odczytu pliku lub deszyfrowania: " + ex.Message);
               
            }

        }

        public void Initialize()
        {
            Console.WriteLine("Szyfrowanie RSA");
            Console.WriteLine("1. Wybierz losowe liczby pierwsze");
            Console.WriteLine("2. Podaj liczby pierwsze ręcznie");

            x = Console.ReadLine();

            while (x != "1" && x != "2")
            {
                Console.WriteLine("Błędna wartość! Wpisz 1 albo 2");
                x = Console.ReadLine();
            }



            switch (x)
            {
                case "1":
                    p = GenerateLargePrime();
                    q = GenerateLargePrime();
                    Console.WriteLine($"p wynosi {p}, q wynosi {q}");
                    Console.ReadKey();
                    break;

                case "2":
                    Console.WriteLine("Podaj pierwszą liczbę pierwszą:");
                    p = BigInteger.Parse(Console.ReadLine());
                    Console.WriteLine("Podaj drugą liczbę pierwszą:");
                    q = BigInteger.Parse(Console.ReadLine());
                    break;

            }

            GenerateKeys();
            Console.Clear();
            Console.WriteLine($"Wygenerowane klucze: \n encryptionKey: {encryptionKey}\n decryptionKey: {decryptionKey}");
        }
        public void Encrypt(string pathin, string pathout)
        {
            using (StreamReader sr = new StreamReader(pathin))
            {
                x = sr.ReadToEnd();
            }
                    encrypted = Encryptascii(x);

                    Console.WriteLine($"Zaszyfrowany tekst to: ");
                    for(int i = 0; i < encrypted.Length; i++)
                    {
                        Console.WriteLine(encrypted[i]);
                    }
            using (StreamWriter sw = new StreamWriter(File.Open(pathout, FileMode.Create)))
            {
                for (int i = 0; i < encrypted.Length; i++)
                    sw.WriteLine(encrypted[i]);
            };
            



            

            

        }

        public BigInteger[] Encryptascii(string x)
        {

            byte[] bytes = Encoding.ASCII.GetBytes(x);


            BigInteger[] result = new BigInteger[bytes.Length];

            for(int i =0; i< bytes.Length; i++)
            {
                result[i] =   BigInteger.ModPow(bytes[i], encryptionKey, n);
            }
            // Szyfrowanie: ciphertext = message^e mod n
           

            return result;
        }

        void GenerateKeys()
        {
            // Obliczanie n - liczby będącej iloczynem dwóch liczb pierwszych
            n = p * q;

            // Obliczanie wartości funkcji Eulera phi(n) = (p-1)*(q-1)
            phi = (p - 1) * (q - 1);

            // Wybór klucza szyfrujacego, który jest liczbą względnie pierwszą z phi
            encryptionKey = ChooseE(phi);

            // Obliczanie klucza deszyfrującego, która jest odwrotnością e modulo phi
            decryptionKey = ModInverse(encryptionKey, phi);
        }

        BigInteger ChooseE(BigInteger phi)
        {

            // Wybieramy pierwszą liczbę względnie pierwszą z phi większą od 2
            BigInteger e = 3;
            while (BigInteger.GreatestCommonDivisor(e, phi) != 1)
            {
                e += 2;
            }
            return e;
        }
        BigInteger ModInverse(BigInteger a, BigInteger n)
        {
            // Obliczanie odwrotności liczby a modulo n
            BigInteger t = 0;
            BigInteger newT = 1;
            BigInteger r = n;
            BigInteger newR = a;

            while (newR != 0)
            {
                BigInteger quotient = r / newR;
                BigInteger tempT = newT;
                newT = t - quotient * newT;
                t = tempT;
                BigInteger tempR = newR;
                newR = r - quotient * newR;
                r = tempR;
            }

            if (r > 1)
            {
                throw new Exception("Liczba a nie ma odwrotności modulo n.");
            }

            if (t < 0)
            {
                t += n;
            }

            return t;
        }
        BigInteger GenerateLargePrime()
        {

            Random random = new Random();
            byte[] randomBytes = new byte[4];
            random.NextBytes(randomBytes);
            BigInteger prime = BigInteger.Abs(new BigInteger(randomBytes));
            while (!IsPrime(prime))
            {
                prime++;
            }
            return prime;
        }

        bool IsPrime(BigInteger number)
        {
            if (number <= 1)
                return false;

            if (number == 2 || number == 3)
                return true;

            if (number % 2 == 0 || number % 3 == 0)
                return false;

            for (BigInteger i = 5; i * i <= number; i += 6)
            {
                if (number % i == 0 || number % (i + 2) == 0)
                    return false;
            }

            return true;
        }
    }

}
