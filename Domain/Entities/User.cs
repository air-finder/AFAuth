using System.Security.Cryptography;
using Application.Helpers;
using Domain.Constants;
using Domain.Entities.Enums;
using Domain.Exceptions;
using Domain.SeedWork.Notification;

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
    public string Login { get; set; }
    public string Hash { get; private set; }
    public string Salt { get; private set; }
    public Guid PersonId { get; set; }
    public UserRole Role { get; set; }

    #region Mapping
    public virtual Person? Person { get; set; } = null;
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