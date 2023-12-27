using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Application.Models;
using Project.DAL.Data;

namespace Project.API.Installers;

/// <summary>
/// Information of database installer
/// CreatedBy: ThiepTT(30/10/2023)
/// </summary>
public class DatabaseInstaller : IWebApplicationBuilderInstaller
{
    /// <summary>
    /// Installer web application builder
    /// </summary>
    /// <param name="builder">WebApplicationBuilder</param>
    /// CreatedBy: ThiepTT(30/10/2023)
    public void InstallerWebApplicationBuilder(WebApplicationBuilder builder)
    {
        ConnectionStrings connectionStrings = new ConnectionStrings();
        builder.Configuration.Bind(nameof(ConnectionStrings), connectionStrings);
        builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionStrings.DefaultConnection));

        builder.Services.AddIdentityCore<IdentityUser>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
        })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DataContext>();
    }
}