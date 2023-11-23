using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using OfficeOpenXml;
using Project.API.Options;
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

        builder.Services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
            config.ApiVersionReader = new UrlSegmentApiVersionReader();
        });

        builder.Services.AddVersionedApiExplorer(config =>
        {
            config.GroupNameFormat = "'v'VVV";
            config.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            string xmlFile = Path.ChangeExtension(Assembly.GetEntryAssembly()!.Location, ".xml");
            options.IncludeXmlComments(xmlFile);
        });

        builder.Services.ConfigureOptions<ConfigSwagger>();

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }
}