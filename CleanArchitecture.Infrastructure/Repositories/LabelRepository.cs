using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Domain.Models;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;

public class LabelRepository : BaseRepository<Label>, ILabelRepository
{
    readonly TaskifyDbContext _context;

    public LabelRepository(TaskifyDbContext context)
        : base(context) => _context = context;


    public IEnumerable<Label> GetWorkspaceLabels(Guid workspaceId)
        => _context.Labels.AsNoTracking().Where(label => label.WorkspaceId == workspaceId);
}
