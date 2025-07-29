using AutoMapper;
using CleanArchitecture.Application.DTOs.LabelDtos;
using CleanArchitecture.Application.DTOs.TaskLabelDtos;
using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Implementations.Services;

public class TaskLabelService : ITaskLabelService
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public TaskLabelService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> AssociateLabelToTask(TaskLabelForCreationDto request)
    {
        var taskLabel = _mapper.Map<TaskLabel>(request);

        await _unitOfWork.TaskLabelRepo.AddAsync(taskLabel);

        return await _unitOfWork.SaveAsync() > 0;
    }

    public async Task<bool> RemovelabelFromTask(Guid taskId, Guid labelId)
    {
        var taskLabel = _unitOfWork.TaskLabelRepo.GetTaskLabel(taskId, labelId);

        if (taskLabel == null)
            return false;

        _unitOfWork.TaskLabelRepo.Remove(taskLabel);

        return await _unitOfWork.SaveAsync() > 0;
    }

    public IEnumerable<LabelDto> TaskLabels(Guid taskId)
    {
        var taskLabels = _unitOfWork.TaskLabelRepo.GetTaskLabels(taskId);

        var taskLabelsDto = _mapper.Map<IEnumerable<LabelDto>>(taskLabels);

        return taskLabelsDto;
    }
}
