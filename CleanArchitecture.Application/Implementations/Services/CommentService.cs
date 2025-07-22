using AutoMapper;
using CleanArchitecture.Application.DTOs.CommentDtos;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CleanArchitecture.Application.Implementations.Services;

public class CommentService : ICommentService
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IHttpContextAccessor _httpContext;

    public CommentService(IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContext,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContext = httpContext;
    }

    public async Task<CommentDto> CreateComment(CommentForCreationDto request)
    {
        var userId = _httpContext.HttpContext?.User.FindFirstValue("id");

        if (string.IsNullOrEmpty(userId)) 
            throw new UnauthorizedAccessException();

        var comment = _mapper.Map<Comment>(request);
        comment.AuthorId = userId;

        await _unitOfWork.CommentRepo.AddAsync(comment);
        await _unitOfWork.SaveAsync();

        var savedComment = _unitOfWork.CommentRepo.GetCommentWithUser(comment.Id);

        var commentDto = _mapper.Map<CommentDto>(savedComment);

        return commentDto;
    }

    public async Task<bool> DeleteComment(Guid commentId)
    {
        var userId = _httpContext.HttpContext?.User.FindFirstValue("id");

        if(string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException();

        var comment = _unitOfWork.CommentRepo.GetEntity(commentId);

        if (comment == null)
            return false;

        if (comment.AuthorId != userId)
            throw new AccessDeniedException();

        _unitOfWork.CommentRepo.Remove(comment);

        return await _unitOfWork.SaveAsync() > 0;

    }

    public CommentDto GetComment(Guid commentId)
    {
        var comment = _unitOfWork.CommentRepo.GetCommentWithUser(commentId);

        if (comment == null)
            return null;

        var commentDto = _mapper.Map<CommentDto>(comment);

        return commentDto;
    }

    public IEnumerable<CommentDto> GetCommentsByTask(Guid taskId)
    {
        var taskComments = _unitOfWork.CommentRepo.GetCommentByTaskWithUser(taskId);

        var taskCommentsDto = _mapper.Map<IEnumerable<CommentDto>>(taskComments);

        return taskCommentsDto;
    }

    public async Task<bool> UpdateComment(Guid commentId, CommentForUpdateDto request)
    {
        var userId = _httpContext.HttpContext?.User.FindFirstValue("id");

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException();

        var comment = _unitOfWork.CommentRepo.GetEntity(commentId);

        if (comment == null)
            return false;

        if (comment.AuthorId != userId)
            throw new AccessDeniedException();

        comment.Content = request.Content;
        comment.UpdatedAt = DateTime.UtcNow;

        return await _unitOfWork.SaveAsync() > 0;
    }
}
