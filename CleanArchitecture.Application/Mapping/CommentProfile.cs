using AutoMapper;
using CleanArchitecture.Application.DTOs.CommentDtos;
using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Mapping;

public class CommentProfile : Profile
{
    public CommentProfile()
    {

        CreateMap<Comment, CommentDto>()
            .ConstructUsing(src => new CommentDto(
                src.Id,
                src.Content,
                src.User.UserName ?? "Unknown",
                src.CreatedAt,
                src.UpdatedAt
            ));

        CreateMap<CommentForCreationDto, Comment>()
            .ForMember(d => d.CreatedAt, opts => opts.MapFrom(src => DateTime.UtcNow));

    }
}
