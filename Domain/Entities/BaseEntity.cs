using Domain.Exceptions;
using Domain.SeedWork.Notification;

namespace Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    protected void CheckEntity()
    {
        if (NotificationsWrapper.HasNotification()) throw new NotificationException();
    }
}