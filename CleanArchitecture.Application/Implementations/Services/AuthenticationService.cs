using AutoMapper;
using CleanArchitecture.Application.DTOs.UserDtos;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.ConfigurationModel;
using CleanArchitecture.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CleanArchitecture.Application.Implementations.Services;

public class AuthenticationService : IAuthenticationService
{
    readonly UserManager<User> _userManager;
    readonly IMapper _mapper;
    User? _user;
    readonly JwtConfiguration _jwtConfiguration;

    public AuthenticationService(UserManager<User> userManager, 
        IOptions<JwtConfiguration> jwtConfiguration, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
        _jwtConfiguration = jwtConfiguration.Value;
    }

    public async Task<TokenDto> CreateToken(bool populateExp)
    {
        List<Claim> claims = await GetClaims();

        SigningCredentials credentials = GetSigningCredentials();

        JwtSecurityToken jwtToken = GetTokenOptions(credentials, claims);

        var refreshToken = GenerateRefreshToken();

        _user.RefreshToken = refreshToken;

        if (populateExp)
            _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);


        await _userManager.UpdateAsync(_user);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return new TokenDto(accessToken, refreshToken);
    }

    private JwtSecurityToken GetTokenOptions(SigningCredentials credentials, List<Claim> claims)
    {
        JwtSecurityToken jwtToken = new JwtSecurityToken(
            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            claims: claims,
            signingCredentials: credentials,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires))
        );

        return jwtToken;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using(var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
    {
        ClaimsPrincipal principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);

        User? user = await _userManager.FindByEmailAsync(principal.Identity.Name);

        if (user is null || user.RefreshToken != tokenDto.RefreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
            throw new Exception("inValid token");

        _user = user;

        return await CreateToken(false);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = _jwtConfiguration.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtConfiguration.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")!)),
            ValidateLifetime = true,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken is null ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    private async Task<List<Claim>> GetClaims()
    {
        List<Claim> claims = [
            new (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new ("id", _user.Id),
            new("email", _user.Email)
        ];

        var userRoles = await _userManager.GetRolesAsync(_user);

        foreach (var role in userRoles)
            claims.Add(new("role", role));

        return claims;
    }

    private SigningCredentials GetSigningCredentials()
    {
        SymmetricSecurityKey key =
            new(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")!));

        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

        return credentials;
    }

    public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
    {
        User user = _mapper.Map<User>(userForRegistration);

        var result = await _userManager.CreateAsync(user, userForRegistration.Password);

        if (result.Succeeded)
        {
            if (await _userManager.Users.AnyAsync())
                await _userManager.AddToRoleAsync(user, "member");
            else
                await _userManager.AddToRoleAsync(user, "admin");
        }
            
        return result;
    }

    public async Task<bool> ValidUser(UserForLoginDto userForLogin)
    {
        _user = await _userManager.FindByEmailAsync(userForLogin.Email);

        bool found =
            _user is not null && await _userManager.CheckPasswordAsync(_user, userForLogin.Password);

        return found;
    }
}
