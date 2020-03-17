using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace EncryptLibrary
{
    public class Encryptor
    {
        private string _initVector;
        private string _passPhrase;
        private int _passwordIterations = 2;
        private int _keySize = 256;
        private string _salt;
        private string defaultInitVector = "@1B2c3D4e5F6g7H8";
        public Encryptor(string passPhrase, string salt)
        {
            PrepareVariables(passPhrase, salt, defaultInitVector);
        }
        public Encryptor(string passPhrase, string salt, string initVector16Chars)
        {
            PrepareVariables(passPhrase, salt, initVector16Chars);
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

            var password = new Rfc2898DeriveBytes(_passPhrase, saltValueBytes, _passwordIterations);
            var intKeySize = (_keySize / 8);
            var keyBytes = password.GetBytes(intKeySize);

            var symetricKey = new RijndaelManaged();
            symetricKey.Mode = CipherMode.CBC;

            var encryptor = symetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream();

            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            var cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();

            return Convert.ToBase64String(cipherTextBytes);
        }
        private string Decode(string cipherText, string saltValue)
        {
            var initVectorBytes = Encoding.ASCII.GetBytes(_initVector);
            var saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            var cipherTextBytes = Convert.FromBase64String(cipherText);

            var password = new Rfc2898DeriveBytes(_passPhrase, saltValueBytes, _passwordIterations);
            var intKeySize = (_keySize / 8);
            var keyBytes = password.GetBytes(intKeySize);

            var symetricKey = new RijndaelManaged();
            symetricKey.Mode = CipherMode.CBC;

            var decryptor = symetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream(cipherTextBytes);

            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var plainTextBytes = new byte[cipherTextBytes.Length];

            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();

            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
        public string Encrypt(string plainText)
        {
            var textToEncrypt= Encode(plainText, _salt);
            return $"--KryptoIt--{textToEncrypt}";
        }
        public string Decrypt(string cipherText)
        {
            if (cipherText.Length > 12 && cipherText.Left(12) == @"--KryptoIt--")
            {
                cipherText = cipherText.Remove(0, 12);
                return Decode(cipherText, _salt);
            }
            return cipherText;
        }
      
    }
}
