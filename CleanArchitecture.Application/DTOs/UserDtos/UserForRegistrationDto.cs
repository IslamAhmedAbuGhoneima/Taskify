using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Application.DTOs.UserDtos;

public record UserForRegistrationDto(
    [EmailAddress(ErrorMessage = "this email is invalid"),
    Required(ErrorMessage = "email field is required")]
    string Email,
    string UserName,
    string? PhotoURL,
    string Password
);
