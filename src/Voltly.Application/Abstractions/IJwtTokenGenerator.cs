namespace Voltly.Application.Abstractions;

public interface IJwtTokenGenerator
{
    /// <summary>Gera o token e devolve a data/hora de expiração</summary>
    string GenerateToken(long userId, string email, string role, out DateTime expiresAt);
}