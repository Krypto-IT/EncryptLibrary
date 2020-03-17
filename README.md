# EncryptLibrary
 Encodes and decodes a string

using EncryptLibrary;
//Using Default Init Vector
var encryptor = new Encryptor(PassPhraseString,SaltString);

 var encrypted = encryptor.Encrypt(text);
 var decrypted = encryptor.Decrypt(encrypted);

//Using Custom Init Vector
var encryptor2 = new Encryptor(PassPhraseString,SaltString,16CharacterInitString);
var encrypted = encryptor2.Encrypt(text);
 var decrypted = encryptor2.Decrypt(encrypted);
