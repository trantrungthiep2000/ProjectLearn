﻿using Project.Application.Models;
using Project.Application.Services;
using Project.Domain.Interfaces.IServices;
using StackExchange.Redis;

namespace Project.API.Installers;

/// <summary>
/// Information of cache installer
/// CreatedBy: ThiepTT(06/11/2023)
/// </summary>
public class CacheInstaller : IWebApplicationBuilderInstaller
{
    /// <summary>
    /// Installer web application builder
    /// </summary>
    /// <param name="builder">WebApplicationBuilder</param>
    /// CreatedBy: ThiepTT(06/11/2023)
    public void InstallerWebApplicationBuilder(WebApplicationBuilder builder)
    {
        RedisConfiguration redisConfiguration = new RedisConfiguration();
        builder.Configuration.Bind(nameof(RedisConfiguration), redisConfiguration);

        builder.Services.AddSingleton(redisConfiguration);

        if (!redisConfiguration.Enabled)
            return;

        builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConfiguration.ConnectionString));
        builder.Services.AddStackExchangeRedisCache(option => option.Configuration = redisConfiguration.ConnectionString);
        builder.Services.AddSingleton<IResponseCacheService, ResponseCacheService>();
    }
}