namespace Application.Interfaces.Persistence;

public interface IUnitOfWork
{
    public ICityRepository Cities { get; }
    
    public Task<int> CommitAsync();
}