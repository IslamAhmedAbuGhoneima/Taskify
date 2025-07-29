using CleanArchitecture.Application.DTOs.LabelDtos;
using CleanArchitecture.Application.DTOs.TaskLabelDtos;

namespace CleanArchitecture.Application.Interfaces.Services;

public interface ITaskLabelService 
{
    Task<bool> AssociateLabelToTask(TaskLabelForCreationDto request);

    Task<bool> RemovelabelFromTask(Guid taskId, Guid labelId);

    IEnumerable<LabelDto> TaskLabels(Guid taskId);
}
