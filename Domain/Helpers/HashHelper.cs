using System.Security.Cryptography;
using System.Text;

namespace Application.Helpers;

public class HashHelper(HashAlgorithm hashAlgorithm, int saltSize)
{
    public (string Hash, string Salt) CreateSaltedHash(string password)
    {
        var salt = new byte[saltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var saltedPasswordBytes = new byte[salt.Length + passwordBytes.Length];
        Buffer.BlockCopy(salt, 0, saltedPasswordBytes, 0, salt.Length);
        Buffer.BlockCopy(passwordBytes, 0, saltedPasswordBytes, salt.Length, passwordBytes.Length);

        var hashBytes = hashAlgorithm.ComputeHash(saltedPasswordBytes);
        var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        var saltHex = BitConverter.ToString(salt).Replace("-", "").ToLower();

        return (hash, saltHex);
    }

    public bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        var salt = Convert.FromHexString(storedSalt);
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var saltedPasswordBytes = new byte[salt.Length + passwordBytes.Length];
        Buffer.BlockCopy(salt, 0, saltedPasswordBytes, 0, salt.Length);
        Buffer.BlockCopy(passwordBytes, 0, saltedPasswordBytes, salt.Length, passwordBytes.Length);

        var hashBytes = hashAlgorithm.ComputeHash(saltedPasswordBytes);
        var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        return hash == storedHash;
    }
}