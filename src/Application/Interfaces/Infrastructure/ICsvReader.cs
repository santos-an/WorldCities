using CSharpFunctionalExtensions;
using Domain.Entities;

namespace Application.Interfaces.Infrastructure;

public interface ICsvReader
{
    public Result<IEnumerable<City>> Read();
}