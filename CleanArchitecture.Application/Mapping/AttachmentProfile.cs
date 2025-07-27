using AutoMapper;
using CleanArchitecture.Application.DTOs.AttachmentDtos;
using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Mapping;

public class AttachmentProfile : Profile
{
    public AttachmentProfile()
    {
        CreateMap<AttachmentForCreationDto, Attachment>()
            .ForMember(d => d.ContentType, opts => opts.MapFrom(src => src.File.ContentType))
            .ForMember(d => d.SizeBytes, opts => opts.MapFrom(src => src.File.Length))
            .ForMember(d => d.FileName, opts => opts.MapFrom(src => src.File.FileName))
            .ForMember(d => d.UploadedAt, opts => opts.MapFrom(src => DateTime.UtcNow));

        CreateMap<Attachment, AttachmentDto>();
    }
}
