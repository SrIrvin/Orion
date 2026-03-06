using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orión.Application.Interfaces;
using Orión.Infrastructure.Persistence;
using Orión.Infrastructure.Repositories;
using Orión.Infrastructure.Services;
using System.IO;
using System.Linq;
using System;
using System.Runtime.Versioning;

namespace Orión.Infrastructure;

[SupportedOSPlatform("windows")]
public static class DependencyInjection
{
    public static IServiceCollection AddOrionPersistence(this IServiceCollection services, IConfiguration configuration, string environment)
    {
        // Registrar Servicio de Configuración Segura
        services.AddSingleton<ISecureConfigService, SecureConfigService>();

        // Obtener configuración preferida del usuario (archivo binario cifrado)
        using var tempProvider = services.BuildServiceProvider();
        var secureConfig = tempProvider.GetRequiredService<ISecureConfigService>();
        var userConfig = secureConfig.LoadConfig();

        services.AddDbContext<OrionDbContext>(options =>
        {
            if (userConfig.Provider.Equals("Access", StringComparison.OrdinalIgnoreCase))
            {
                var connString = userConfig.GetConnectionString();
                
                // Asegurar directorio
                EnsureDirectoryExists(connString);
                
                options.UseJet(connString);
            }
            else
            {
                var connString = userConfig.GetConnectionString();
                options.UseNpgsql(connString);
            }

            options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        });

        services.AddScoped<IOrionDbContext>(provider => provider.GetRequiredService<OrionDbContext>());
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

        return services;
    }

    private static void EnsureDirectoryExists(string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString)) return;

        var dataSourcePart = connectionString.Split(';')
            .FirstOrDefault(x => x.Trim().StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase));

        if (dataSourcePart != null)
        {
            var filePath = dataSourcePart.Split('=')[1].Trim();
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
