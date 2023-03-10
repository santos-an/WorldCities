namespace Application.Interfaces.Persistence;

public interface IUnitOfWork
{
    public ICityRepository Cities { get; }
    public ITokenRepository Tokens { get; }
    public IUserRepository Users { get; }
    
    public Task<int> CommitAsync();
}