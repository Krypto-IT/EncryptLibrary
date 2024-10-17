namespace EncryptLibrary
{
    public interface IEncryptor
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
  
}