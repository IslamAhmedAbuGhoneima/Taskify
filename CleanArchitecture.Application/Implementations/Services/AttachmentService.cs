using AutoMapper;
using CleanArchitecture.Application.DTOs.AttachmentDtos;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CleanArchitecture.Application.Implementations.Services;

public class AttachmentService : IAttachmentService
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _httpContext;
    readonly IWebHostEnvironment _webHostEnvironment;
    readonly IMapper _mapper;

    public AttachmentService(IUnitOfWork unitOfWork, 
        IHttpContextAccessor httpContext,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _httpContext = httpContext;
        _webHostEnvironment = webHostEnvironment;
        _mapper = mapper;
    }

    public async Task<bool> DeleteAttachment(Guid attachmentId)
    {
        var userId = _httpContext.HttpContext?.User.FindFirstValue("id");

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("you are not authorized to make this action");

        var attachment = _unitOfWork.AttachmentRepo.GetEntity(attachmentId);

        if (attachment == null)
            return false;

        if (attachment.UploadedByUserId != userId)
            throw new AccessDeniedException();

        if (!File.Exists(attachment.BlobPath))
            throw new FileNotFoundException("we can not find this file");

        File.Delete(attachment.BlobPath);

        _unitOfWork.AttachmentRepo.Remove(attachment);

        return await _unitOfWork.SaveAsync() > 0;
    }

    public AttachmentDto GetAttachment(Guid attachmentId)
    {
        var attachment = _unitOfWork.AttachmentRepo.GetEntity(attachmentId);

        var attachmentDto = _mapper.Map<AttachmentDto>(attachment);

        return attachmentDto;
    }

    public IEnumerable<AttachmentDto> GetTaskAttachments(Guid taskId)
    {
        var taskAttachments = _unitOfWork.AttachmentRepo.GetTaskAttachments(taskId);

        var taskAttachmentsDto = _mapper.Map<IEnumerable<AttachmentDto>>(taskAttachments);

        return taskAttachmentsDto;
    }

    public async Task<AttachmentDto> UploadAttachment(AttachmentForCreationDto request)
    {
        var userId = _httpContext.HttpContext?.User.FindFirstValue("id");
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("you are not authorized to make this action");

        var attachmentPath = SaveAttachment(request.File);

        var attachment = _mapper.Map<Attachment>(request);
        attachment.BlobPath = attachmentPath;
        attachment.UploadedByUserId = userId;

        await _unitOfWork.AttachmentRepo.AddAsync(attachment);
        await _unitOfWork.SaveAsync();

        var attachmentDto = _mapper.Map<AttachmentDto>(attachment);

        return attachmentDto;
    }

    private string SaveAttachment(IFormFile file)
    {

        if (file.Length > 5_242_880)
            throw new FileSizeException();

        var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Attachments");

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var fileName = Guid.NewGuid().ToString();
        var fileExtenstion = Path.GetExtension(file.FileName);

        var attachmentPath = Path.Combine(path, $"{fileName}{fileExtenstion}");

        using var stream = new FileStream(attachmentPath, FileMode.Create);
        file.CopyTo(stream);

        var bloadPath =
            Path.Combine(_webHostEnvironment.ContentRootPath, "Attachments", $"{fileName}{fileExtenstion}");

        return bloadPath;
    }
}
