using Project.API.Options;
using Project.Application.MappingProfiles;
using MediatR;
using Project.Application.Identities.Commands;

namespace Project.API.Installers;

/// <summary>
/// Information of web application builder installer
/// CreatedBy: ThiepTT(30/10/2023)
/// </summary>
public class WebApplicationBuilderInstaller : IWebApplicationBuilderInstaller
{
    /// <summary>
    /// Installer web application builder
    /// </summary>
    /// <param name="builder">WebApplicationBuilder</param>
    /// CreatedBy: ThiepTT(30/10/2023)
    public void InstallerWebApplicationBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        builder.Services.ConfigureOptions<ConfigSwagger>();

        builder.Services.AddAutoMapper(new[] { typeof(IdentityMapping) });

        builder.Services.AddMediatR(new[] { typeof(RegisterCommand) });
    }
}