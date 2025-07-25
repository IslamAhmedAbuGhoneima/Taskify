using AutoMapper;
using CleanArchitecture.Application.DTOs.NotificationDtos;
using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Mapping;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<NotificationForCreation, Notification>()
            .ForMember(d => d.CreatedAt, opts => opts.MapFrom(src => DateTime.UtcNow));

        CreateMap<Notification, NotificationDto>();

    }
}
