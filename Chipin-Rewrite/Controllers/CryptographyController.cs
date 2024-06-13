using Chipin_Rewrite.Utility.Encryption;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace Chipin_Rewrite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptographyController : ControllerBase
    {
        private readonly IEncryptionService _encryptionService;
        public CryptographyController(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }
        [HttpPost]
        [Route("encrypt")]
        //endpoint to take in the an external product without any foreign keys and encrypt it
        async public Task<IActionResult> Encrypt([FromBody] Models.Entities.ExternalProduct product)
        {
            //encrypt the product
            byte[] keyBytes = GenerateRandomKey(32);                                        //Not in use
            string key = "jgfz2Cnh6dKwF/O15SI7XrZKxvjfT2E6A5qzbj2FQhM=";
            Console.WriteLine(key);
            string jsonProduct = JsonConvert.SerializeObject(product);
            string encryptedProduct = _encryptionService.EncryptAES(jsonProduct, key);
            return Ok(encryptedProduct);
        }

        static byte[] GenerateRandomKey(int keyLength)                                      //Not in use
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[keyLength];
                rng.GetBytes(key);
                return key;
            }
        }
    }
}
