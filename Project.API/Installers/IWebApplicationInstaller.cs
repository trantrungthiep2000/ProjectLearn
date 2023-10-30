namespace Project.API.Installers;

/// <summary>
/// Information of interface web application installer
/// CreatedBy: ThiepTT(30/10/2023)
/// </summary>
public interface IWebApplicationInstaller : IInstaller
{
    /// <summary>
    /// Installer web application 
    /// </summary>
    /// <param name="app">WebApplication</param>
    /// CreatedBy: ThiepTT(30/10/2023)
    public void InstallerWebApplication(WebApplication app);
}