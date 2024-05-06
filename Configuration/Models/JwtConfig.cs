namespace ptdn_net.Configuration.Models;

public class JwtConfig
{
    public string? Issuer { get; set; }

    public string? Audience { get; set; }

    public string? SecretKey { get; set; }
}