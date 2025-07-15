using CleanArchitecture.Application.DTOs.UserDtos;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Interfaces.Services;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);

    Task<TokenDto> CreateToken(bool populateExp);

    Task<bool> ValidUser(UserForLoginDto userForLogin);
}