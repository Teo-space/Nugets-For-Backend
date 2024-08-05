using System.Security.Cryptography;


/// <summary>
/// Hashing And Validating passwords
/// </summary>
public static partial class PasswordHasher
{
    static readonly int keySize = 512 / 8;
    static readonly int iterations = 100_000;
    static readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
}