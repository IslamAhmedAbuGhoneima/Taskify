using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    TaskifyDbContext _context;

    public BaseRepository(TaskifyDbContext context)
        => _context = context;

    public async Task AddAsync(T entity)
        => await _context.AddAsync(entity);

    public async Task AddRangeAsync(IEnumerable<T> entities)
        => await _context.AddRangeAsync(entities);

    public IEnumerable<T> GetAll()
        => _context.Set<T>().AsNoTracking();

    public T GetEntity(Guid Id)
        => _context.Find<T>(Id);

    public void Remove(T entity)
        => _context.Remove(entity);

    public void RemoveRange(IEnumerable<T> entities)
        => _context.RemoveRange(entities);

    public void Update(T entity)
        => _context.Update(entity);
}
