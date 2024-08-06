using Domain.Constants;
using Domain.Exceptions;
using Domain.SeedWork.Notification;
using static System.String;

namespace Domain.Common.User;

public class LoginRequest
{
    public string Login { get; set; }
    public string Password { get; set; }

    public void Check()
    {
        if(IsNullOrEmpty(Login)) NotificationsWrapper.AddNotification(NotificationMessages.NotEmptyEntity( "Login"));
        if(IsNullOrEmpty(Password)) NotificationsWrapper.AddNotification(NotificationMessages.NotEmptyEntity( "Password"));
        if (NotificationsWrapper.HasNotification()) throw new NotificationException();
    }
}