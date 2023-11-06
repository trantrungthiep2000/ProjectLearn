using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Project.Application.Models;
using System.Text;

namespace Project.API.Installers;

/// <summary>
/// Information of authentication installer
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class AuthenticationInstaller : IWebApplicationBuilderInstaller
{
    /// <summary>
    /// Installer web application builder
    /// </summary>
    /// <param name="builder">WebApplicationBuilder</param>
    /// CreatedBy: ThiepTT(31/10/2023)
    public void InstallerWebApplicationBuilder(WebApplicationBuilder builder)
    {
        JwtSettings jwtSettings = new JwtSettings();
        builder.Configuration.Bind(nameof(JwtSettings), jwtSettings);

        IConfigurationSection jwtSection = builder.Configuration.GetSection(nameof(JwtSettings));
        builder.Services.Configure<JwtSettings>(jwtSection);

        builder.Services
            .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SigningKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudiences = jwtSettings.Audiences,
                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                    LifetimeValidator = LifetimeValidator,
                };
                options.Audience = jwtSettings.Audiences[0];
                options.ClaimsIssuer = jwtSettings.Issuer;
            });
    }

    /// <summary>
    /// Life time validator
    /// </summary>
    /// <param name="notBefore">NotBefore</param>
    /// <param name="expires">Expires</param>
    /// <param name="token">SecurityToken</param>
    /// <param name="params">TokenValidationParameters</param>
    /// <returns>True | False</returns>
    /// CreatedBy: ThiepTT(06/11/2023)
    private bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
    {
        if (expires != null)
        {
            return expires > DateTime.UtcNow;
        }
        return false;
    }
}