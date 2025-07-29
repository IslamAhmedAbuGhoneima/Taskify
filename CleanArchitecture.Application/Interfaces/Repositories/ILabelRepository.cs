using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Interfaces.Repositories;

public interface ILabelRepository : IBaseRepository<Label>
{
    IEnumerable<Label> GetWorkspaceLabels(Guid workspaceId);
}
