using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Orión.Application.DTOs;
using Orión.Infrastructure.Services;
using System.IO;
using System.Runtime.Versioning;
using Xunit;

namespace Orión.Tests.Infrastructure;

[SupportedOSPlatform("windows")]
public class SecureConfigServiceTests : IDisposable
{
    private IConfiguration _config;

    public SecureConfigServiceTests()
    {
        // Usar una configuración real en lugar de mock para evitar errores con extensiones como GetValue
        var myConfiguration = new Dictionary<string, string?>
        {
            {"DbProvider", "PostgreSQL"},
            {"Environment", "Development"},
            {"ConnectionStrings:DefaultConnection", "Host=localhost;Database=OrionDB"}
        };

        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();
    }

    [Fact]
    public void SaveAndLoad_ShouldMaintainDataIntegrity()
    {
        // Arrange - Usamos archivo de test independiente
        var service = new SecureConfigService(_config, "db_config_test.bin");
        var originalConfig = new DbConfigurationDto
        {
            Provider = "PostgreSQL",
            Host = "test-host",
            Password = "SecurePassword123",
            Username = "test-user"
        };

        // Act
        service.SaveConfig(originalConfig);
        var loadedConfig = service.LoadConfig();

        // Assert
        loadedConfig.Should().NotBeNull();
        loadedConfig.Host.Should().Be(originalConfig.Host);
        
        // Cleanup
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var testPath = Path.Combine(appData, "Orión", "db_config_test.bin");
        if (File.Exists(testPath)) File.Delete(testPath);
    }

    [Fact]
    public void LoadConfig_WhenFileDoesNotExist_ShouldReturnDefaults()
    {
        // Arrange - Usamos archivo que no existe
        var service = new SecureConfigService(_config, "non_existent_config.bin");

        // Act
        var config = service.LoadConfig();

        // Assert
        config.Should().NotBeNull();
        config.Provider.Should().Be("PostgreSQL");
    }

    [Fact]
    public void ClearSession_ShouldResetPersistenceFields()
    {
        // Arrange
        var service = new SecureConfigService(_config, "db_config_clear_test.bin");
        var config = new DbConfigurationDto
        {
            RememberMe = true,
            LastUserId = 1,
            SessionExpiry = DateTime.UtcNow.AddDays(1)
        };
        service.SaveConfig(config);

        // Act
        service.ClearSession();
        var clearedConfig = service.LoadConfig();

        // Assert
        clearedConfig.RememberMe.Should().BeFalse();
        clearedConfig.LastUserId.Should().BeNull();
        clearedConfig.SessionExpiry.Should().BeNull();

        // Cleanup
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var testPath = Path.Combine(appData, "Orión", "db_config_clear_test.bin");
        if (File.Exists(testPath)) File.Delete(testPath);
    }

    public void Dispose()
    {
        // Cleanup if needed
    }
}
