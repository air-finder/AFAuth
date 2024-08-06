using Domain.Common;

namespace Application;

public interface IBaseCrudService<T> where T : class
{
    public Task<T?> GetByIdAsync(Guid id);
    public Task<IEnumerable<T>> GetListAsync(int pageIndex, int pageSize);
    public Task CreateAsync(T entity);
    public Task UpdateAsync(T entity);
    public Task DeleteAsync(Guid id);
}