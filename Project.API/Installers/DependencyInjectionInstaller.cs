using Microsoft.AspNetCore.Identity;
using Project.Application.Services;
using Project.Domain.Aggregates;
using Project.Domain.Interfaces.IRepositories;
using Project.Domain.Interfaces.IServices;
using Project.Infrastructure.Repositories;

namespace Project.API.Installers;

/// <summary>
/// Information of dependency injection installer
/// CreatedBy: ThiepTT(01/11/2023)
/// </summary>
public class DependencyInjectionInstaller : IWebApplicationBuilderInstaller
{
    /// <summary>
    /// Installer web application builder
    /// </summary>
    /// <param name="builder">WebApplicationBuilder</param>
    /// CreatedBy: ThiepTT(01/11/2023)
    public void InstallerWebApplicationBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IdentityService>();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddScoped<IIdentityUserRepository<IdentityUser>, IdentityUserRepository>();

        builder.Services.AddScoped<IUserProfileRepository<UserProfile>, UserProfileRepository>();

        builder.Services.AddScoped<IProductRepository<Product>, ProductRepository>();
    }
}