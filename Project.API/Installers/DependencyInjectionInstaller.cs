using Project.Application.Services;

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
    }
}
