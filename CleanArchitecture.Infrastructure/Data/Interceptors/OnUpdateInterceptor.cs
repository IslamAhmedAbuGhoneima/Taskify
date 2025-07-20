using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskEntity = CleanArchitecture.Domain.Models.Task;


namespace CleanArchitecture.Infrastructure.Data.Interceptors;

public class OnUpdateInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context == null)
            return result;

        foreach (var entity in eventData.Context.ChangeTracker.Entries<TaskEntity>())
        {
            if (entity is {  State: EntityState.Modified })
                entity.Entity.UpdatedAt = DateTime.UtcNow;  
        }

        return base.SavingChanges(eventData, result);
    }
}
