using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Orión.Application.Interfaces;
using Orión.Application.Services;
using Orión.Domain.Entities;
using Orión.Infrastructure.Persistence;
using Orión.Infrastructure.Repositories;
using Xunit;

namespace Orión.Tests.Application;

public class ProveedorServiceTests
{
    private (OrionDbContext context, IProveedorService service) GetDependencies()
    {
        var options = new DbContextOptionsBuilder<OrionDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new OrionDbContext(options);
        var repository = new GenericRepository<Proveedor>(context);
        var service = new ProveedorService(repository, context);
        return (context, service);
    }

    [Fact]
    public async Task GetOrCreateByNameAsync_Should_Return_Existing_If_Name_Matches_CaseInsensitive()
    {
        // Arrange
        var (context, service) = GetDependencies();
        var existing = new Proveedor { Nombre = "Test Proveedor", Activo = true };
        await context.Proveedores.AddAsync(existing);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetOrCreateByNameAsync("test proveedor");

        // Assert
        result.IdProveedor.Should().Be(existing.IdProveedor);
        (await context.Proveedores.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task GetOrCreateByNameAsync_Should_Create_New_If_Not_Exists()
    {
        // Arrange
        var (context, service) = GetDependencies();

        // Act
        var result = await service.GetOrCreateByNameAsync("Nuevo Proveedor");

        // Assert
        result.Should().NotBeNull();
        result.Nombre.Should().Be("Nuevo Proveedor");
        (await context.Proveedores.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task GetAllDtoAsync_Should_Filter_By_Status()
    {
        // Arrange
        var (context, service) = GetDependencies();
        context.Proveedores.AddRange(
            new Proveedor { Nombre = "P1", Activo = true },
            new Proveedor { Nombre = "P2", Activo = false }
        );
        await context.SaveChangesAsync();

        // Act
        var resultActive = await service.GetAllDtoAsync(includeInactive: false);
        var resultAll = await service.GetAllDtoAsync(includeInactive: true);

        // Assert
        resultActive.Should().HaveCount(1);
        resultAll.Should().HaveCount(2);
    }

    [Fact]
    public async Task ToggleStatusAsync_Should_Change_Activo_Property()
    {
        // Arrange
        var (context, service) = GetDependencies();
        var prov = new Proveedor { Nombre = "ToggleTest", Activo = true };
        await context.Proveedores.AddAsync(prov);
        await context.SaveChangesAsync();

        // Act
        await service.ToggleStatusAsync(prov.IdProveedor);

        // Assert
        var updated = await context.Proveedores.FindAsync(prov.IdProveedor);
        updated!.Activo.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_Should_Persist_Changes()
    {
        // Arrange
        var (context, service) = GetDependencies();
        var prov = new Proveedor { Nombre = "Original", Activo = true };
        await context.Proveedores.AddAsync(prov);
        await context.SaveChangesAsync();

        // Act
        prov.Nombre = "Modificado";
        await service.UpdateAsync(prov);

        // Assert
        var result = await context.Proveedores.FindAsync(prov.IdProveedor);
        result!.Nombre.Should().Be("Modificado");
    }
}
