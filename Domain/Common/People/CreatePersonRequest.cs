using Domain.Constants;
using Domain.Entities.Enums;
using Domain.SeedWork.Notification;
using static System.String;

namespace Domain.Common.People;

public class CreatePersonRequest
{
    public string Login { get; set; } = Empty;
    public string Password { get; set; } = Empty;
    public string Name { get; set; } = Empty;
    public string Email { get; set; } = Empty;
    public string? PersonCode { get; set; }
    public string? Phone { get; set; }
    public DateTime? Birthday { get; set; }
    public Gender Gender { get; set; }

    public void Check()
    {
        if(IsNullOrEmpty(Login)) NotificationsWrapper.AddNotification(NotificationMessages.NotEmptyEntity( "Login"));
        if(IsNullOrEmpty(Password)) NotificationsWrapper.AddNotification(NotificationMessages.NotEmptyEntity( "Password"));
        if(IsNullOrEmpty(Name)) NotificationsWrapper.AddNotification(NotificationMessages.NotEmptyEntity( "Name"));
        if(IsNullOrEmpty(Email)) NotificationsWrapper.AddNotification(NotificationMessages.NotEmptyEntity( "Email"));
    }
}