using Microsoft.Extensions.Configuration;
using Npgsql;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;
using System.Data.OleDb;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Runtime.Versioning;

namespace Orión.Infrastructure.Services;

[SupportedOSPlatform("windows")]
public class SecureConfigService : ISecureConfigService
{
    private readonly string _configPath;
    private readonly IConfiguration _appConfig;

    public SecureConfigService(IConfiguration appConfig) : this(appConfig, "db_config.bin") { }

    // Constructor interno para facilitar pruebas unitarias sin sobrescribir config real
    internal SecureConfigService(IConfiguration appConfig, string fileName)
    {
        _appConfig = appConfig;
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dir = Path.Combine(appData, "Orión");
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        _configPath = Path.Combine(dir, fileName);
    }

    public DbConfigurationDto LoadConfig()
    {
        if (!File.Exists(_configPath))
        {
            return GetDefaultConfig();
        }

        try
        {
            byte[] encryptedData = File.ReadAllBytes(_configPath);
            byte[] decryptedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
            string json = Encoding.UTF8.GetString(decryptedData);
            return JsonSerializer.Deserialize<DbConfigurationDto>(json) ?? GetDefaultConfig();
        }
        catch
        {
            return GetDefaultConfig();
        }
    }

    public void SaveConfig(DbConfigurationDto config)
    {
        string json = JsonSerializer.Serialize(config);
        byte[] data = Encoding.UTF8.GetBytes(json);
        byte[] encryptedData = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
        File.WriteAllBytes(_configPath, encryptedData);
    }

    public void ClearSession()
    {
        var config = LoadConfig();
        config.RememberMe = false;
        config.LastUserId = null;
        config.SessionExpiry = null;
        SaveConfig(config);
    }

    public async Task<bool> TestConnection(DbConfigurationDto config)
    {
        var connString = config.GetConnectionString();
        
        if (config.Provider.Equals("Access", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                using var conn = new OleDbConnection(connString);
                await conn.OpenAsync();
                return true;
            }
            catch { return false; }
        }
        else
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                await conn.OpenAsync();
                return true;
            }
            catch { return false; }
        }
    }

    private DbConfigurationDto GetDefaultConfig()
    {
        var provider = _appConfig.GetValue<string>("DbProvider") ?? "PostgreSQL";
        var env = _appConfig.GetValue<string>("Environment") ?? "Development";
        var isProd = env.Equals("Production", StringComparison.OrdinalIgnoreCase);
        var connStringName = provider.Equals("Access", StringComparison.OrdinalIgnoreCase) 
            ? "AccessConnection" 
            : (env == "Staging" ? "StagingConnection" : "DefaultConnection");
        
        var connString = _appConfig.GetConnectionString(connStringName);

        if (provider.Equals("Access", StringComparison.OrdinalIgnoreCase))
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var realPath = connString?.Replace("{Documents}", documentsPath);
            var filePath = realPath?.Split(';')
                .FirstOrDefault(x => x.Trim().StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))?
                .Split('=')[1].Trim();

            return new DbConfigurationDto { Provider = "Access", AccessFilePath = filePath, IsProduction = isProd };
        }
        else
        {
            // Parse basic Npgsql string if needed, or just return basic
            return new DbConfigurationDto 
            { 
                Provider = "PostgreSQL", 
                Host = "localhost", 
                DatabaseName = "DB_Orion", 
                Username = "admin", 
                Port = 5433,
                IsProduction = isProd
            };
        }
    }
}
