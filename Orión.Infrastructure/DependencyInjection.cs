using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orión.Application.Interfaces;
using Orión.Infrastructure.Persistence;
using Orión.Infrastructure.Repositories;
using System.IO;
using System.Linq;
using System;

namespace Orión.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOrionPersistence(this IServiceCollection services, IConfiguration configuration, string environment)
    {
        var dbProvider = configuration.GetValue<string>("DbProvider") ?? "PostgreSQL";

        services.AddDbContext<OrionDbContext>(options =>
        {
            if (dbProvider.Equals("Access", StringComparison.OrdinalIgnoreCase))
            {
                var rawConnString = configuration.GetConnectionString("AccessConnection");
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var connString = rawConnString?.Replace("{Documents}", documentsPath);
                
                // Asegurar directorio
                EnsureDirectoryExists(connString);
                
                options.UseJet(connString);
            }
            else
            {
                var connStringName = environment == "Staging" ? "StagingConnection" : "DefaultConnection";
                var connString = configuration.GetConnectionString(connStringName);
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
