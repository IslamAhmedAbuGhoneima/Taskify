using CleanArchitecture.Application.Implementations.Services;
using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Application.Mapping;
using CleanArchitecture.Domain.Models;
using CleanArchitecture.Domain.ConfigurationModel;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CleanArchitecture.Infrastructure.Data.Interceptors;
using Microsoft.Extensions.FileProviders;

namespace CleanArchitecture.API.Extensions;

public static class ServiceExtensions
{
    public static void AddUnitOfWork(this IServiceCollection services)
        => services.AddScoped<IUnitOfWork, UnitOfWork>();

    public static void AddServiceManager(this IServiceCollection services)
        => services.AddScoped<IBaseServiceManager, BaseServiceManager>();

    public static void AddDbContext(this IServiceCollection services, IConfiguration config)
        => services.AddDbContext<TaskifyDbContext>(opts =>
        opts.UseSqlServer(config.GetConnectionString("TaskifyConnection"))
        .AddInterceptors(new OnUpdateInterceptor()));

    public static void AddAutoMapper(this IServiceCollection services)
        => services.AddAutoMapper(cfg => cfg.AddMaps(typeof(WorkspaceProfile).Assembly));


    public static void AddIdentity(this IServiceCollection services)
        => services.AddIdentity<User, IdentityRole>(opts =>
        {
            opts.User.RequireUniqueEmail = true;
            opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
            opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

            opts.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
        }).AddEntityFrameworkStores<TaskifyDbContext>()
        .AddDefaultTokenProviders();

    public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration config)
    => services.Configure<JwtConfiguration>(config.GetSection("JWTSettings"));

    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(opts =>
        {
            // Check JWT Header
            opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            // unauthorize
            opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opts =>
        {
            opts.SaveToken = true;
            opts.RequireHttpsMetadata = false;
            opts.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["JWTSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["JWTSettings:Audience"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")!))
            };
        });
    }


    public static void AddStaticFiles(this WebApplication app, IWebHostEnvironment environment)
    {
        string path = Path.Combine(environment.ContentRootPath, "Attachments");

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(path),
            RequestPath = "/Attachments"
        });
    }
}
