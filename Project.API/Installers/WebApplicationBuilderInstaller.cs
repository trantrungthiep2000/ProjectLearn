using MediatR;
using Project.API.Options;
using Project.Application.Identities.Commands;
using Project.Application.MappingProfiles;
using System.Reflection;

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

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
    }
}