using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Project.API.Options;

/// <summary>
/// Information of config swagger
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class ConfigSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    /// <summary>
    /// Config swagger
    /// </summary>
    /// <param name="provider">IApiVersionDescriptionProvider</param>
    /// CreatedBy: ThiepTT(07/11/2023)
    public ConfigSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// Configure
    /// </summary>
    /// <param name="options">SwaggerGenOptions</param>
    /// CreatedBy: ThiepTT(31/10/2023)
    public void Configure(SwaggerGenOptions options)
    {
        foreach (ApiVersionDescription description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
        }

        var schema = GetJwtSecuritySchema();
        options.AddSecurityDefinition(schema.Reference.Id, schema);
        options.AddSecurityRequirement(new OpenApiSecurityRequirement{ { schema, new string[0] } });
    }

    /// <summary>
    /// Create version info
    /// </summary>
    /// <param name="description">ApiVersionDescription</param>
    /// <returns>OpenApiInfo</returns>
    /// CreatedBy: ThiepTT(07/11/2023)
    private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = SystemConfig.Title,
            Version = description.ApiVersion.ToString()
        };

        if (description.IsDeprecated)
        {
            info.Description = SystemConfig.Description;
        }

        return info;
    }

    /// <summary>
    /// Get json web token security schema
    /// </summary>
    /// <returns>OpenApiSecurityScheme</returns>
    /// CreatedBy: ThiepTT(31/10/2023)
    private OpenApiSecurityScheme GetJwtSecuritySchema()
    {
        return new OpenApiSecurityScheme
        {
            Name = "Jwt Authentication",
            Description = "Provide a JWT Bearer",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };
    }
}