using CSharpFunctionalExtensions;
using Domain.Cities;

namespace Application.Interfaces.Infrastructure;

public interface ICsvReader
{
    public Result<IEnumerable<City>> Read();
}