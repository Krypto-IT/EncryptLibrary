namespace EncryptLibrary
{
    public interface IEncodeDecode
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
  
}