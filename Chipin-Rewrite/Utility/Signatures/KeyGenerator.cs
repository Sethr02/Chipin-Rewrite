using System.Security.Cryptography;

namespace Chipin_Rewrite.Utility.Signatures
{
    public class KeyGenerator
    {
        public void GenerateKeys()
        {
            using (RSA rsa = RSA.Create())
            {
                // Generate an RSA key pair with a key size of 2048 bits
                rsa.KeySize = 2048;

                // Export the private key
                RSAParameters privateKey = rsa.ExportParameters(true);

                // Export the public key
                RSAParameters publicKey = rsa.ExportParameters(false);

                // Print the private and public keys
                Console.WriteLine("Private Key:");
                Console.WriteLine(Convert.ToBase64String(privateKey.D));
                Console.WriteLine(Convert.ToBase64String(privateKey.Modulus));

                Console.WriteLine("\nPublic Key:");
                Console.WriteLine(Convert.ToBase64String(publicKey.Exponent));
                Console.WriteLine(Convert.ToBase64String(publicKey.Modulus));
            }
        }

    }
}
