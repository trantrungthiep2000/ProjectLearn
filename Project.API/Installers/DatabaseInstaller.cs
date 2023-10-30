using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Data;

namespace Project.API.Installers;

/// <summary>
/// Information of database installer
/// CreatedBy: ThiepTT(30/10/2023)
/// </summary>
public class DatabaseInstaller : IWebApplicationBuilderInstaller
{
    public void InstallerWebApplicationBuilder(WebApplicationBuilder builder)
    {
        var connectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionStrings));

        builder.Services.AddIdentityCore<IdentityUser>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 5;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        })
            .AddEntityFrameworkStores<DataContext>();
    }
}