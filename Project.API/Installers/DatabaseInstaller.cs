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
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
        })
            .AddEntityFrameworkStores<DataContext>();
    }
}