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
    /// CreatedBy; ThiepTT(30/10/2023)
    public void InstallerWebApplication(WebApplication app)
    {
        app.UseSwagger();

        app.UseSwaggerUI();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();
    }
}