using CSharpFunctionalExtensions;
using Domain;

namespace Application;

public interface IReader
{
    public Result<IEnumerable<City>> Read();
}