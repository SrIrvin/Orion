using FluentAssertions;
using Orión.Application.DTOs;
using Xunit;

namespace Orión.Tests.Application;

public class DbConfigurationDtoTests
{
    [Fact]
    public void GetConnectionString_ForAccess_ShouldReturnCorrectFormat()
    {
        // Arrange
        var config = new DbConfigurationDto
        {
            Provider = "Access",
            AccessFilePath = "C:\\DB\\data.accdb"
        };

        // Act
        var connString = config.GetConnectionString();

        // Assert
        connString.Should().Be("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\DB\\data.accdb");
    }

    [Fact]
    public void GetConnectionString_ForPostgreSQL_WithoutSSL_ShouldReturnCorrectFormat()
    {
        // Arrange
        var config = new DbConfigurationDto
        {
            Provider = "PostgreSQL",
            Host = "localhost",
            Port = 5432,
            DatabaseName = "OrionDB",
            Username = "admin",
            Password = "password123",
            SslMode = false
        };

        // Act
        var connString = config.GetConnectionString();

        // Assert
        connString.Should().Contain("Host=localhost")
                  .And.Contain("Port=5432")
                  .And.Contain("Database=OrionDB")
                  .And.Contain("Username=admin")
                  .And.Contain("Password=password123")
                  .And.Contain("SSL Mode=Disable");
    }

    [Fact]
    public void GetConnectionString_ForPostgreSQL_WithSSL_ShouldReturnCorrectFormat()
    {
        // Arrange
        var config = new DbConfigurationDto
        {
            Provider = "PostgreSQL",
            Host = "remote-host",
            Port = 5433,
            DatabaseName = "ProdDB",
            Username = "user",
            Password = "pass",
            SslMode = true
        };

        // Act
        var connString = config.GetConnectionString();

        // Assert
        connString.Should().Contain("SSL Mode=Require")
                  .And.Contain("Port=5433")
                  .And.Contain("Host=remote-host");
    }

    [Fact]
    public void IsProduction_DefaultValue_ShouldBeFalse()
    {
        // Act
        var config = new DbConfigurationDto();

        // Assert
        config.IsProduction.Should().BeFalse();
    }
}
