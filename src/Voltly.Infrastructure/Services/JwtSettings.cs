namespace Voltly.Infrastructure.Services;

public sealed class JwtSettings
{
    public string Key              { get; init; } = null!;
    public string Issuer           { get; init; } = null!;
    public string Audience         { get; init; } = null!;
    public int    ExpirationMinutes { get; init; } = 60;
}