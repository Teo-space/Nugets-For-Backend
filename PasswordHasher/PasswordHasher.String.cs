using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Hashing And Validating passwords
/// </summary>
public static partial class PasswordHasher
{
    public static class String
    {
        /// <summary>
        /// Hashing Result
        /// </summary>
        /// <param name="Hash">128 Length</param>
        /// <param name="Salt">128 Length</param>
        public readonly record struct HashedPassword(string Hash, string Salt);
        public static HashedPassword Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return new HashedPassword(Convert.ToHexString(hash), Convert.ToHexString(salt));
        }
        /// <summary>
        /// Verify Password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hash">128 Length</param>
        /// <param name="salt">128 Length</param>
        /// <returns>IsValid</returns>
        public static bool Verify(string password, string hash, string salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, Convert.FromHexString(salt), iterations, hashAlgorithm, keySize);

            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }

    }
}
