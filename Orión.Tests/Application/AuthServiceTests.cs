using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Orión.Application.Interfaces;
using Orión.Application.Services;
using Orión.Domain.Entities;
using Orión.Infrastructure.Persistence;
using Orión.Infrastructure.Repositories;
using Xunit;

namespace Orión.Tests.Application;

public class AuthServiceTests
{
    private (OrionDbContext, IRepository<Usuario>) GetDependencies()
    {
        var options = new DbContextOptionsBuilder<OrionDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new OrionDbContext(options);
        var repository = new GenericRepository<Usuario>(context);
        return (context, repository);
    }

    [Fact]
    public async Task RegisterAsync_Should_Hash_Password()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new AuthService(repository, context);

        // Act
        var user = await service.RegisterAsync("testuser", "password123", "test@test.com", "Admin");

        // Assert
        user.PasswordHash.Should().NotBe("password123");
        user.NombreUsuario.Should().Be("testuser");
    }

    [Fact]
    public async Task LoginAsync_Should_Return_User_For_Valid_Credentials()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new AuthService(repository, context);
        await service.RegisterAsync("validuser", "correct_pass", null, "Operador");

        // Act
        var result = await service.LoginAsync("validuser", "correct_pass");

        // Assert
        result.Should().NotBeNull();
        result!.NombreUsuario.Should().Be("validuser");
    }

    [Fact]
    public async Task LoginAsync_Should_Return_Null_For_Invalid_Credentials()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new AuthService(repository, context);
        await service.RegisterAsync("validuser", "correct_pass", null, "Operador");

        // Act
        var result = await service.LoginAsync("validuser", "wrong_pass");

        // Assert
        result.Should().BeNull();
    }
}
