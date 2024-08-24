using System.Security.Cryptography;
using Application.Helpers;
using Domain.Common.People;
using Domain.Constants;
using Domain.Entities.Enums;
using Domain.SeedWork.Notification;

namespace Domain.Entities;

public class User : BaseEntity
{
    public User() { }
    public User(string login, string password)
    {
        if (string.IsNullOrWhiteSpace(login)) NotificationsWrapper.AddNotification(NotificationMessages.NotEmptyEntity(nameof(Login)));
        if (string.IsNullOrWhiteSpace(password)) NotificationsWrapper.AddNotification(NotificationMessages.NotEmptyEntity("Password"));
        CheckEntity();

        Login = login;
        (Hash, Salt) = HashPassword(password);
        Role = UserRole.User;
    }
    public string Login { get; init; }
    public string Hash { get; private set; }
    public string Salt { get; private set; }
    public Guid PersonId { get; set; }
    public UserRole Role { get; init; }

    #region Mapping
    public virtual Person? Person { get; set; }
    #endregion

    #region Methods
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
    public void UpdatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) NotificationsWrapper.AddNotification(NotificationMessages.NotEmptyEntity("Password"));
        CheckEntity();
        (Hash, Salt) = HashPassword(password);
    }
    #endregion

    public static implicit operator User(AddUserRequest request) => new(request.Login, request.Password);
}