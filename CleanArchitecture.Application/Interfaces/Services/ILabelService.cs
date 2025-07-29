using CleanArchitecture.Application.DTOs.LabelDtos;

namespace CleanArchitecture.Application.Interfaces.Services;

public interface ILabelService
{
    Task<LabelDto> CreateLabel(LabelForCreationDto request);

    LabelDto GetLabel(Guid labelId);

    IEnumerable<LabelDto> GetWorkspaceLabels(Guid workspaceId);

    Task<bool> UpdateLabel(Guid labelId, LabelForUpdateDto request);

    Task<bool> DeleteLabel(Guid labelId);
}
