using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Application.DTOs.AttachmentDtos;

public record AttachmentForCreationDto(Guid TaskId,IFormFile File);
