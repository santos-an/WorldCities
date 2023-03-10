namespace Application.Interfaces.Persistence;

public interface ITokenRepository
{
    Task GetRefreshTokenBy(string token);
}