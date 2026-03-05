using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Orión.Domain.Entities;
using Orión.Infrastructure.Persistence;
using Xunit;

namespace Orión.Tests.Domain;

public class BaseEntityTests
{
    private OrionDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<OrionDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new OrionDbContext(options);
    }

    private class TestEntity : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }

    [Fact]
    public async Task SaveChangesAsync_Should_Set_CreatedAt_On_Insert()
    {
        // Arrange
        // Nota: Usamos una entidad real ya que TestEntity no está en el DbContext
        var context = GetDbContext();
        var tecnico = new Tecnico { IdPersonal = 999, NombreApellido = "Test" };

        // Act
        context.Tecnicos.Add(tecnico);
        await context.SaveChangesAsync();

        // Assert
        tecnico.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        tecnico.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public async Task SaveChangesAsync_Should_Set_UpdatedAt_On_Update()
    {
        // Arrange
        var context = GetDbContext();
        var tecnico = new Tecnico { IdPersonal = 888, NombreApellido = "Initial" };
        context.Tecnicos.Add(tecnico);
        await context.SaveChangesAsync();
        
        var originalCreated = tecnico.CreatedAt;
        await Task.Delay(10); // Asegurar diferencia de tiempo

        // Act
        tecnico.NombreApellido = "Updated";
        await context.SaveChangesAsync();

        // Assert
        tecnico.CreatedAt.Should().Be(originalCreated);
        tecnico.UpdatedAt.Should().NotBeNull();
        tecnico.UpdatedAt.Should().BeAfter(originalCreated);
    }
}
