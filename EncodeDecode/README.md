# EncodeDecode Class

The `EncodeDecode` class provides encryption and decryption functionality using AES (Advanced Encryption Standard) with a customizable initialization vector and passphrase. It is designed to securely encode and decode data using a salted encryption approach.

## Features

- **AES encryption**: Securely encrypts plaintext data using the AES algorithm in CBC mode.
- **AES decryption**: Decodes AES-encrypted ciphertext back into plaintext.
- **Salted encryption**: Includes salt values for added security during encryption.
- **Customizable parameters**: You can specify the passphrase, salt, and initialization vector (IV) for encryption.

## Requirements

- .NET 5 or higher
- A minimum passphrase length of 8 characters
- A minimum salt length of 8 characters
- An initialization vector (IV) that must be exactly 16 characters

## How to Use

### 1. Using Dependency Injection

In your app settings add the configuration settings:
```json
 "EncodeDecodeConfiguration": {
    "PasswordIterations": 2,
    "Salt": "ThisIsYourSalt",
    "InitVector": "@1B2c3D4e5F6g7H8",
    "Passphrase": "DoNotUseForPasswords"
  }
```
Register the service in your program.cs file
```csharp

    services.Configure<EncodeDecodeConfiguration>(context.Configuration.GetSection(EncodeDecodeConfiguration.Name));
    services.AddScoped<IEncodeDecode, EncodeDecode>();

```
Inject the service where you need it:
```csharp
     private readonly IEncodeDecode _encryptor;
      public YourClassConstructor(IEncodeDecode encrypt)
        {
            _encryptor = encrypt;
        }
        public void test()
        {
            Console.WriteLine("Enter Text");
            var text = Console.ReadLine();
            var encrypted = _encryptor.Encrypt(text);
            var decrypted =_encryptor.Decrypt(encrypted);
            Console.WriteLine(encrypted);
            Console.WriteLine("****************");
            Console.WriteLine(decrypted);
        }
```
### 2. Without Dependency Injection You May Initialize the EncodeDecode

You can initialize the `EncodeDecode` class with the following parameters:

```csharp
string passPhrase = "YourPassphrase"; // At least 8 characters
string salt = "YourSaltValue";        // At least 8 characters
string initVector = "@1B2c3D4e5F6g7H8"; // Must be exactly 16 characters

EncodeDecode encryptor = new EncodeDecode(passPhrase, salt, initVector);

