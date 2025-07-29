using AutoMapper;
using CleanArchitecture.Application.DTOs.LabelDtos;
using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Implementations.Services;

public class LabelService : ILabelService
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public LabelService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LabelDto> CreateLabel(LabelForCreationDto request)
    {
        var label = _mapper.Map<Label>(request);

        await _unitOfWork.LableRepo.AddAsync(label);

        await _unitOfWork.SaveAsync();

        var labelDto = _mapper.Map<LabelDto>(label);

        return labelDto;
    }

    public async Task<bool> DeleteLabel(Guid labelId)
    {
        var label = _unitOfWork.LableRepo.GetEntity(labelId);

        if (label == null)
            return false;

        _unitOfWork.LableRepo.Remove(label);
        return await _unitOfWork.SaveAsync() > 0;
    }

    public LabelDto GetLabel(Guid labelId)
    {
        var label = _unitOfWork.LableRepo.GetEntity(labelId);

        var labelDto = _mapper.Map<LabelDto>(label);

        return labelDto;
    }

    public IEnumerable<LabelDto> GetWorkspaceLabels(Guid workspaceId)
    {
        var workspaceLabels = _unitOfWork.LableRepo.GetWorkspaceLabels(workspaceId);

        var workspaceLabelsDto = _mapper.Map<IEnumerable<LabelDto>>(workspaceLabels);

        return workspaceLabelsDto;
    }

    public async Task<bool> UpdateLabel(Guid labelId, LabelForUpdateDto request)
    {
        var label = _unitOfWork.LableRepo.GetEntity(labelId);

        if (label == null)
            return false;

        _mapper.Map(request, label);

        return await _unitOfWork.SaveAsync() > 0;
    }
}
