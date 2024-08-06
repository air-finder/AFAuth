using System.Security.Cryptography;
using Application.Helpers;
using Domain.Entities.Enums;
using static System.String;

namespace Domain.Entities;

public class User : BaseEntity
{
    public User() { }
    public User(string login, string password)
    {
        Login = login;
        (Hash, Salt) = HashPassword(password);
        Role = UserRole.User;
    }
    public string Login { get; private set; } = Empty;
    public string Hash { get; private set; } = Empty;
    public string Salt { get; private set; } = Empty;
    public Guid PersonId { get; set; }
    public UserRole Role { get; private set; }

    #region Mapping
    public virtual Person? Person { get; set; }
    #endregion
    
    private readonly HashAlgorithm _hashAlgorithm = SHA256.Create();

    private (string, string) HashPassword(string password)
    {
        var hashHelper = new HashHelper(_hashAlgorithm, 16);
        return hashHelper.CreateSaltedHash(password);
    }

    public bool CheckPassword(string password)
    {
        var hashHelper = new HashHelper(_hashAlgorithm, 16);
        return hashHelper.VerifyPassword(password, Hash, Salt);
    }
}