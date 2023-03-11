using Domain.Cities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Persistence;

public interface IDbInitializer
{
    void GenerateData();
    
    IReadOnlyList<City> Cities { get; }
    IReadOnlyList<IdentityRole> Roles { get; }
    IReadOnlyList<IdentityUser> Users { get; }
    IReadOnlyList<IdentityUserRole<string>> UserRoles { get; }
}