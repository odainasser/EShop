namespace Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(Guid userId, string email, IList<string> roles, IList<string> permissions);
    string GenerateRefreshToken();
}
