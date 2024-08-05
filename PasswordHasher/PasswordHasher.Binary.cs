using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Hashing And Validating passwords
/// </summary>
public static partial class PasswordHasher
{
    public static class Binary
    {
        /// <summary>
        /// Hashing Result
        /// </summary>
        /// <param name="Hash">64 Length</param>
        /// <param name="Salt">64 Length</param>
        public readonly record struct HashedPassword(byte[] Hash, byte[] Salt);
        public static HashedPassword Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return new HashedPassword(hash, salt);
        }

        /// <summary>
        /// Verify Password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hash">64 Length</param>
        /// <param name="salt">64 Length</param>
        /// <returns>IsValid</returns>
        public static bool Verify(string password, byte[] hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);

            return CryptographicOperations.FixedTimeEquals(hashToCompare, hash);
        }

    }
}

