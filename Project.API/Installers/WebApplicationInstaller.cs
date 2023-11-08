using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Project.API.Middlewares;

namespace Project.API.Installers;

/// <summary>
/// Information of web application installer
/// CreatedBy; ThiepTT(30/10/2023)
/// </summary>
public class WebApplicationInstaller : IWebApplicationInstaller
{
    /// <summary>
    /// Installer web application
    /// </summary>
    /// <param name="app">WebApplication</param>
    /// CreatedBy: ThiepTT(30/10/2023)
    public void InstallerWebApplication(WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Project {description.ApiVersion}");
                options.RoutePrefix = string.Empty;
            }
        });

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();
    }
}