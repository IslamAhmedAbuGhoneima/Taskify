using CleanArchitecture.Application.DTOs.UserDtos;
using CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(IBaseServiceManager serviceManager) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> CreateUser(UserForRegistrationDto user)
    {
        var result = 
            await serviceManager.AuthenticationService.RegisterUser(user);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.TryAddModelError(error.Code, error.Description);

            return BadRequest(ModelState);
        }

        return Created();
    }

    [HttpPost("login")]
    public async Task<IActionResult> AuthenticateUser(UserForLoginDto userForLoginDto)
    {
        var validUser = await serviceManager.AuthenticationService.ValidUser(userForLoginDto);

        if (!validUser)
            return Unauthorized();

        var tokenDto =
            await serviceManager.AuthenticationService.CreateToken(false);

        return Ok(tokenDto);
    }
}
