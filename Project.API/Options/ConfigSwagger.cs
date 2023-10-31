using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Project.API.Options;

/// <summary>
/// Information of config swagger
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class ConfigSwagger : IConfigureOptions<SwaggerGenOptions>
{
    /// <summary>
    /// Configure
    /// </summary>
    /// <param name="options">SwaggerGenOptions</param>
    /// CreatedBy: ThiepTT(31/10/2023)
    public void Configure(SwaggerGenOptions options)
    {
        var schema = GetJwtSecuritySchema();
        options.AddSecurityDefinition(schema.Reference.Id, schema);
        options.AddSecurityRequirement(new OpenApiSecurityRequirement{ { schema, new string[0] } });
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