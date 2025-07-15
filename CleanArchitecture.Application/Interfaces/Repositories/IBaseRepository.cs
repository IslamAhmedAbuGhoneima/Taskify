namespace CleanArchitecture.Application.Interfaces.Repositories;

public interface IBaseRepository<T> 
{
    IEnumerable<T> GetAll();

    T GetEntity(Guid Id);

    Task AddAsync(T entity);

    Task AddRangeAsync(IEnumerable<T> entities);

    void Update(T entity);

    void Remove(T entity);

    void RemoveRange(IEnumerable<T> entities);
}
