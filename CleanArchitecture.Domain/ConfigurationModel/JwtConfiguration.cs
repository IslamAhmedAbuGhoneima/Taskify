namespace CleanArchitecture.Domain.ConfigurationModel;

public class JwtConfiguration
{
    public string Section { get; set; } = "JwtSettings";

    public string? Issuer { get; set; }

    public string? Audience { get; set; }

    public int? Expires { get; set; }
}
