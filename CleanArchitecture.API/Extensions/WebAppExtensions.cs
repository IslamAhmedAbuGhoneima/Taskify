using Microsoft.Extensions.FileProviders;

namespace CleanArchitecture.API.Extensions;

public static class WebAppExtensions
{
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
