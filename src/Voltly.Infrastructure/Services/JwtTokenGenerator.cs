using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Voltly.Application.Abstractions;

namespace Voltly.Infrastructure.Services;

public sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _cfg;
    public JwtTokenGenerator(IConfiguration cfg) => _cfg = cfg;

    public string GenerateToken(long userId, string email, string role, out DateTime expiresAt)
    {
        var issuer    = _cfg["Jwt:Issuer"];
        var audience  = _cfg["Jwt:Audience"];
        var secretKey = _cfg["Jwt:SecretKey"]!;

        expiresAt = DateTime.UtcNow.AddMinutes(
            int.Parse(_cfg["Jwt:ExpiresMinutes"] ?? "60"));

        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,  userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email,email),
            new Claim(ClaimTypes.Role,              role.ToUpperInvariant()),
            new Claim(JwtRegisteredClaimNames.Jti,  Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: expiresAt,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}