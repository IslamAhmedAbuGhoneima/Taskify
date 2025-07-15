using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Application.DTOs.UserDtos;

public record UserForLoginDto(
    [EmailAddress(ErrorMessage = "Invalid email address")]
    string Email,
    string Password
);
