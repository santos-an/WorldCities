using CSharpFunctionalExtensions;
using Domain;
using Domain.Entities;

namespace Application.Interfaces.Infrastructure;

public interface IReader
{
    public Result<IEnumerable<City>> Read();
}