using Domain.Constants;
using Domain.Exceptions;
using Domain.SeedWork.Notification;
using static System.String;

namespace Domain.Common.User;

public class UpdatePasswordRequest(string password)
{
    public string Password { get; set; } = password;
    public void Check()
    {
        if (IsNullOrEmpty(Password)) NotificationsWrapper.AddNotification(NotificationMessages.NotEmptyEntity("Password"));
        if (NotificationsWrapper.HasNotification()) throw new NotificationException();
    }
}