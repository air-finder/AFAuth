using Domain;
using Domain.Common;
using Domain.Exceptions;
using Domain.SeedWork.Notification;
using Microsoft.EntityFrameworkCore;

namespace Application;

public class BaseCrudService<T>(IBaseRepository<T> repository) : IBaseCrudService<T> where T : class
{
    protected void AddNotification(string message) => NotificationsWrapper.AddNotification(message);
    protected void CheckNotification() {
        if (NotificationsWrapper.HasNotification()) throw new NotificationException();
    }
    public async Task<T?> GetByIdAsync(Guid id)
        => await repository.GetByIDAsync(id);

    public async Task<IEnumerable<T>> GetListAsync(int pageIndex, int pageSize)
        => await repository.GetAll().AsNoTracking().Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

    public async Task CreateAsync(T entity)
        => await repository.InsertWithSaveChangesAsync(entity);

    public async Task UpdateAsync(T entity) 
        => await repository.UpdateWithSaveChangesAsync(entity);

    public async Task DeleteAsync(Guid id)
        => await repository.DeleteAsync(id);
}