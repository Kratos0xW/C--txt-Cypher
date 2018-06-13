using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;


namespace crypto
{
    class Program
    {
        const string publicKey = "<RSAKeyValue><Modulus>21wEnTU+mcD2w0Lfo1Gv4rtcSWsQJQTNa6gio05AOkV/Er9w3Y13Ddo5wGtjJ19402S71HUeN0vbKILLJdRSES5MHSdJPSVrOqdrll/vLXxDxWs/U0UT1c8u6k/Ogx9hTtZxYwoeYqdhDblof3E75d9n2F0Zvf6iTb4cI7j6fMs=</Modulus><Exponent>AQAB</Exponent><P>/aULPE6jd5IkwtWXmReyMUhmI/nfwfkQSyl7tsg2PKdpcxk4mpPZUdEQhHQLvE84w2DhTyYkPHCtq/mMKE3MHw==</P><Q>3WV46X9Arg2l9cxb67KVlNVXyCqc/w+LWt/tbhLJvV2xCF/0rWKPsBJ9MC6cquaqNPxWWEav8RAVbmmGrJt51Q==</Q><DP>8TuZFgBMpBoQcGUoS2goB4st6aVq1FcG0hVgHhUI0GMAfYFNPmbDV3cY2IBt8Oj/uYJYhyhlaj5YTqmGTYbATQ==</DP><DQ>FIoVbZQgrAUYIHWVEYi/187zFd7eMct/Yi7kGBImJStMATrluDAspGkStCWe4zwDDmdam1XzfKnBUzz3AYxrAQ==</DQ><InverseQ>QPU3Tmt8nznSgYZ+5jUo9E0SfjiTu435ihANiHqqjasaUNvOHKumqzuBZ8NRtkUhS6dsOEb8A2ODvy7KswUxyA==</InverseQ><D>cgoRoAUpSVfHMdYXW9nA3dfX75dIamZnwPtFHq80ttagbIe4ToYYCcyUz5NElhiNQSESgS5uCgNWqWXt5PnPu4XmCXx6utco1UVH8HGLahzbAnSy6Cj3iUIQ7Gj+9gQ7PkC434HTtHazmxVgIR5l56ZjoQ8yGNCPZnsdYEmhJWk=</D></RSAKeyValue>";

        [STAThread]
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            string opcion = "";
            while (opcion != "3" && opcion != "exit")
            {
                Console.Clear();
                Console.WriteLine("C# message cypher");
                Console.WriteLine(" ");
                Console.WriteLine("Selecy an option ... ");
                Console.WriteLine(" ");
                Console.WriteLine("   1 >> Encrypt message");
                Console.WriteLine("   2 >> Decrypt message");
                Console.WriteLine("   3 >> Exit");
                Console.WriteLine(" ");
                Console.Write(">> ");

                opcion = Console.ReadLine();
                Console.Clear();
                Console.WriteLine(" ");

                string customtext = string.Empty;
                string tbl = string.Empty;
                switch (opcion)
                {
                    case "1":
                        Console.Clear();
                        Console.Write("Type message to encrypt >> ");
                        customtext = Console.ReadLine();
                        if (customtext != "")
                        {
                            encrypt(customtext);
                        }
                        else
                        {
                            Console.WriteLine("Message cannot be empty");
                            Console.ReadLine();
                        }
                        break;
                    case "2":
                        Console.Clear();
                        Console.Write("Type message to decrypt >> ");
                        customtext = Console.ReadLine();
                        if (customtext != "")
                        {
                            decrypt(customtext);
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("Message cannot be empty");
                            Console.ReadLine();
                        }
                        break;

                    case "3":
                        break;

                    case "exit":
                        break;

                    default:
                        Console.WriteLine("Incorrect option ...");
                        break;

                }
            }
        }
        [STAThread]
        private static void encrypt(string msg)
        {
            var testData = Encoding.UTF8.GetBytes(msg);
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    // client encrypting data with public key issued by server
                    rsa.FromXmlString(publicKey);
                    var encryptedData = rsa.Encrypt(testData, true);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    Console.WriteLine(base64Encrypted);
                    // server decrypting data with private key
                    rsa.FromXmlString(publicKey);
                    var resultBytes = Convert.FromBase64String(base64Encrypted);
                    var decryptedBytes = rsa.Decrypt(resultBytes, true);
                    var decryptedData = Encoding.UTF8.GetString(decryptedBytes);

                    Console.WriteLine("Encrypted message: ");
                    Console.WriteLine(decryptedData);
                    Console.WriteLine("Copy message to clipboard y/n?");
                    var opt = Console.ReadLine();
                    if (opt == "y")
                    {
                        Clipboard.SetText(base64Encrypted);
                        Console.WriteLine("Message copied to clipboard");
                    }
                    Console.WriteLine("Press any key to return main...");
                    Console.ReadKey();
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
        [STAThread]
        private static void decrypt(string msg)
        {
            var cypherText = msg;
            var bytesCypherText = Convert.FromBase64String(cypherText);
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    // client encrypting data with public key issued by server
                    rsa.FromXmlString(publicKey);
                    var encryptedData = rsa.Decrypt(bytesCypherText, true);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    // server decrypting data with private key
                    rsa.FromXmlString(publicKey);
                    var decryptedBytes = rsa.Decrypt(bytesCypherText, true);
                    var decryptedData = Encoding.UTF8.GetString(decryptedBytes);
                    Console.WriteLine("Encrypted message: ");
                    Console.WriteLine(decryptedData);
                    Console.WriteLine("Copy message to clipboard y/n?");
                    var opt = Console.ReadLine();
                    if (opt == "y")
                    {
                        Clipboard.SetText(decryptedData);
                        Console.WriteLine("Message copied to clipboard");
                    }
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
    }
}
