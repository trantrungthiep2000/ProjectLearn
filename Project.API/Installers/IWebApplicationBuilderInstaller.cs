namespace Project.API.Installers;

/// <summary>
/// Information of interface web application builder installer
/// CreatedBy: ThiepTT(30/10/2023)
/// </summary>
public interface IWebApplicationBuilderInstaller : IInstaller
{
    /// <summary>
    /// Installer web application builder 
    /// </summary>
    /// <param name="builder">WebApplicationBuilder</param>
    /// CreatedBy: ThiepTT(30/10/2023)
    public void InstallerWebApplicationBuilder(WebApplicationBuilder builder);
}