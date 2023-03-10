using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Persistence;

public interface IDbInitializer
{
    public IReadOnlyList<City> GetCities();
    public IReadOnlyList<IdentityRole> GetRoles();
}