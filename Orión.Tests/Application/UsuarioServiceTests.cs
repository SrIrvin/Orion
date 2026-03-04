using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Orión.Application.Interfaces;
using Orión.Application.Services;
using Orión.Domain.Entities;
using Orión.Infrastructure.Persistence;
using Orión.Infrastructure.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Orión.Tests.Application;

public class UsuarioServiceTests
{
    private (OrionDbContext, IRepository<Usuario>) GetDependencies()
    {
        var options = new DbContextOptionsBuilder<OrionDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;
        var context = new OrionDbContext(options);
        var repository = new GenericRepository<Usuario>(context);
        return (context, repository);
    }

    [Fact]
    public async Task CreateAsync_Should_Hash_Password_And_Set_Defaults()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new UsuarioService(repository, context);
        var usuario = new Usuario { NombreUsuario = "testuser", Rol = "Operador" };

        // Act
        await service.CreateAsync(usuario, "password123");

        // Assert
        var result = await context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == "testuser");
        result.Should().NotBeNull();
        result!.PasswordHash.Should().NotBe("password123"); // Debe estar hasheada
        result.Activo.Should().BeTrue();
    }

    [Fact]
    public async Task ToggleStatusAsync_Should_Switch_Active_State()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new UsuarioService(repository, context);
        var usuario = new Usuario { IdUsuario = 1, NombreUsuario = "test", Activo = true, Rol = "Admin", PasswordHash = "hash" };
        await context.Usuarios.AddAsync(usuario);
        await context.SaveChangesAsync();

        // Act
        await service.ToggleStatusAsync(1);

        // Assert
        var result = await context.Usuarios.FindAsync(1);
        result!.Activo.Should().BeFalse();
    }

    [Fact]
    public async Task ResetPasswordAsync_Should_Update_Hash()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new UsuarioService(repository, context);
        var usuario = new Usuario { IdUsuario = 1, NombreUsuario = "test", PasswordHash = "old_hash", Rol = "Admin" };
        await context.Usuarios.AddAsync(usuario);
        await context.SaveChangesAsync();

        // Act
        await service.ResetPasswordAsync(1, "new_password");

        // Assert
        var result = await context.Usuarios.FindAsync(1);
        result!.PasswordHash.Should().NotBe("old_hash");
        result.PasswordHash.Should().NotBe("new_password");
    }
}
