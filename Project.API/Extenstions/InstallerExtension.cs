using Project.API.Installers;

namespace Project.API.Extenstions;

/// <summary>
/// Information of installer extension
/// CreatedBy: ThiepTT(30/10/2023)
/// </summary>
public static class InstallerExtension
{
    /// <summary>
    /// Web application builder installer
    /// </summary>
    /// <param name="builder">WebApplicationBuilder</param>
    /// <param name="type">Type</param>
    /// CreatedBy: ThiepTT(30/10/2023)
    public static void WebApplicationBuilderInstaller(this WebApplicationBuilder builder, Type type)
    {
        IEnumerable<IWebApplicationBuilderInstaller> installers = GetInstallers<IWebApplicationBuilderInstaller>(type);

        foreach (var installer in installers)
        {
            installer.InstallerWebApplicationBuilder(builder);
        }
    }

    /// <summary>
    /// Web application installer
    /// </summary>
    /// <param name="app">WebApplication</param>
    /// <param name="type">Type</param>
    /// CreatedBy: ThiepTT(30/10/2023)
    public static void WebApplicationInstaller(this WebApplication app, Type type)
    {
        IEnumerable<IWebApplicationInstaller> installers = GetInstallers<IWebApplicationInstaller>(type);

        foreach (var installer in installers)
        {
            installer.InstallerWebApplication(app);
        }
    }

    /// <summary>
    /// Get installers
    /// </summary>
    /// <typeparam name="T">TEntity</typeparam>
    /// <param name="type">Type</param>
    /// <returns>IEnumerable of TEntity</returns>
    /// CreatedBy: ThiepTT(30/10/2023)
    private static IEnumerable<T> GetInstallers<T>(Type type) where T : IInstaller
    {
        return type.Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(T)) && !t.IsAbstract && !t.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<T>();
    }
}