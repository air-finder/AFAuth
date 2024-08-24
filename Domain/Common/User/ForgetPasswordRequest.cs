using Domain.Constants;
using Domain.Exceptions;
using Domain.SeedWork.Notification;

namespace Domain.Common.User;
public class ForgetPasswordRequest(string user, string? code, string? password)
{
    public string User { get; set; } = user;
    public string? Code { get; set; } = code;
    public string? Password { get; set; } = password;

    public void Check()
    {
        if (string.IsNullOrWhiteSpace(User)) NotificationsWrapper.AddNotification(NotificationMessages.NotEmptyEntity(nameof(User)));
        if (Code is "") NotificationsWrapper.AddNotification(NotificationMessages.NotEmptyEntity(nameof(Code)));
        if (Password is "") NotificationsWrapper.AddNotification(NotificationMessages.NotEmptyEntity(nameof(Password)));
        if (NotificationsWrapper.HasNotification()) throw new NotificationException();
    }
}