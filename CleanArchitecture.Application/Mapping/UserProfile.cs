using AutoMapper;
using CleanArchitecture.Application.DTOs.UserDtos;
using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserForRegistrationDto, User>()
            .ForMember(d => d.CreatedAt,
            opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}
