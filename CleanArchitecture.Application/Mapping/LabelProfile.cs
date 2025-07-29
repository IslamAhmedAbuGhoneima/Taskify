using AutoMapper;
using CleanArchitecture.Application.DTOs.LabelDtos;
using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Mapping;

public class LabelProfile : Profile
{
    public LabelProfile()
    {
        CreateMap<LabelForCreationDto, Label>()
            .ForMember(d => d.CreatedAt, opts => opts.MapFrom(src => DateTime.UtcNow));

        CreateMap<Label, LabelDto>();

        CreateMap<LabelForUpdateDto, Label>();
    }
}
