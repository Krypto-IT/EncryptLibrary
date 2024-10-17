using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Microsoft.Extensions.Options;

namespace EncryptLibrary
{
    public class EncodeDecode : IEncodeDecode
    {
        private string _initVector;
        private string _passPhrase;
        private int _passwordIterations = 2;
        private int _keySize = 256;
        private string _salt;
        private string defaultInitVector = "@1B2c3D4e5F6g7H8";
        private readonly EncodeDecodeConfiguration _config;
        public EncodeDecode(string passPhrase, string salt)
        {
            PrepareVariables(passPhrase, salt, defaultInitVector);
        }
        public EncodeDecode(string passPhrase, string salt, string initVector16Chars)
        {
            PrepareVariables(passPhrase, salt, initVector16Chars);
        }
        public EncodeDecode(IOptions<EncodeDecodeConfiguration> config)
        {
            _config = config.Value;
            PrepareVariables(_config.Passphrase, _config.Salt, _config.InitVector);
        }
        private void PrepareVariables(string passPhrase, string salt, string initVector16Chars)
        {
            if (passPhrase.Length < 8)
                throw new FieldAccessException("passPhrase needs to be at least 8 characters");
            if (salt.Length < 8)
                throw new FieldAccessException("Salt needs to be at least 8 characters");
            if (initVector16Chars.Length != 16)
                throw new FieldAccessException("initVector must be 16 characters, to use default value do not include this");
            _passPhrase = passPhrase;
            _salt = salt;
            _initVector = initVector16Chars;
        }
        private string Encode(string plainText, string saltValue)
        {

            var initVectorBytes = Encoding.ASCII.GetBytes(_initVector);
            var saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Generate the key using Rfc2898DeriveBytes
            var password = new Rfc2898DeriveBytes(_passPhrase, saltValueBytes, _passwordIterations);
            var intKeySize = (_keySize / 8);
            var keyBytes = password.GetBytes(intKeySize);

            // Create AES instance
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = keyBytes;
                aesAlg.IV = initVectorBytes; // Initialization vector

                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                using (var memoryStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();

                    var cipherTextBytes = memoryStream.ToArray();
                    return Convert.ToBase64String(cipherTextBytes);
                }
            }
        }
        private string Decode(string cipherText, string saltValue)
        {
            var initVectorBytes = Encoding.ASCII.GetBytes(_initVector);
            var saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            var cipherTextBytes = Convert.FromBase64String(cipherText);

            // Generate the key using Rfc2898DeriveBytes
            var password = new Rfc2898DeriveBytes(_passPhrase, saltValueBytes, _passwordIterations);
            var intKeySize = (_keySize / 8);
            var keyBytes = password.GetBytes(intKeySize);

            // Create AES instance
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = keyBytes;
                aesAlg.IV = initVectorBytes; // Initialization vector

                using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                using (var memoryStream = new MemoryStream(cipherTextBytes))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (var resultStream = new MemoryStream())
                {
                    cryptoStream.CopyTo(resultStream);
                    var plainTextBytes = resultStream.ToArray();

                    return Encoding.UTF8.GetString(plainTextBytes);
                }
            }
        }

        public string Encrypt(string plainText)
        {
            var textToEncrypt= Encode(plainText, _salt);
            return $"--KryptoIt--{textToEncrypt}";
        }
        public string Decrypt(string cipherText)
        {
            if (cipherText.Length > 12 && cipherText.StartsWith("--KryptoIt--"))
            {
                cipherText = cipherText.Remove(0, 12);
                return Decode(cipherText, _salt);
            }
            return cipherText;
        }
      
    }
}
