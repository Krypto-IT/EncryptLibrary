# Encryptor Class

The `Encryptor` class provides encryption and decryption functionality using AES (Advanced Encryption Standard) with a customizable initialization vector and passphrase. It is designed to securely encode and decode data using a salted encryption approach.

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

### 1. Initialize the Encryptor

You can initialize the `Encryptor` class with the following parameters:

```csharp
string passPhrase = "YourPassphrase"; // At least 8 characters
string salt = "YourSaltValue";        // At least 8 characters
string initVector = "@1B2c3D4e5F6g7H8"; // Must be exactly 16 characters

Encryptor encryptor = new Encryptor(passPhrase, salt, initVector);
