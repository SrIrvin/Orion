using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Orión.Application.Interfaces;
using Orión.Application.Services;
using Orión.Domain.Entities;
using Orión.Infrastructure.Persistence;
using Orión.Infrastructure.Repositories;
using Xunit;

namespace Orión.Tests.Application;

public class ComponenteServiceTests
{
    private (IOrionDbContext context, IRepository<Componente> repository) GetDependencies()
    {
        var options = new DbContextOptionsBuilder<OrionDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new OrionDbContext(options);
        var repository = new GenericRepository<Componente>(context);
        return (context, repository);
    }

    [Fact]
    public async Task GetByMaquinariaIdDtoAsync_Should_Return_Only_Related_Components()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new ComponenteService(repository, context);

        var tipo = new TipoComponente { IdTipoComponente = 1, NombreTipo = "Motor" };
        var estado = new EstadoComponente { IdEstado = 1, DescripcionEstado = "Activo" };
        
        var comp1 = new Componente 
        { 
            IdComponente = "C1", 
            NombreComponente = "Bomba", 
            IdMaquinaria = "MAQ1",
            TipoComponente = tipo,
            EstadoComponente = estado
        };
        
        var comp2 = new Componente 
        { 
            IdComponente = "C2", 
            NombreComponente = "Valvula", 
            IdMaquinaria = "MAQ2", // Otra máquina
            TipoComponente = tipo,
            EstadoComponente = estado
        };

        await context.Set<Componente>().AddRangeAsync(comp1, comp2);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetByMaquinariaIdDtoAsync("MAQ1");

        // Assert
        result.Should().HaveCount(1);
        result.First().IdComponente.Should().Be("C1");
        result.First().TipoComponenteNombre.Should().Be("Motor");
    }

    [Fact]
    public async Task CreateAsync_Should_Persist_Componente()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new ComponenteService(repository, context);
        var componente = new Componente { IdComponente = "NEW-C", NombreComponente = "Nuevo", IdMaquinaria = "M1" };

        // Act
        await service.CreateAsync(componente);

        // Assert
        var result = await context.Set<Componente>().FindAsync("NEW-C");
        result.Should().NotBeNull();
        result!.IdMaquinaria.Should().Be("M1");
    }

    [Fact]
    public async Task GetByMaquinariaIdDtoAsync_Should_Filter_Inactive_By_Default()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new ComponenteService(repository, context);

        var tipo = new TipoComponente { IdTipoComponente = 1, NombreTipo = "Motor" };
        var estado = new EstadoComponente { IdEstado = 1, DescripcionEstado = "Activo" };
        
        var compActivo = new Componente { IdComponente = "ACT", IdMaquinaria = "M1", Activo = true, TipoComponente = tipo, EstadoComponente = estado };
        var compInactivo = new Componente { IdComponente = "INA", IdMaquinaria = "M1", Activo = false, TipoComponente = tipo, EstadoComponente = estado };

        await context.Set<Componente>().AddRangeAsync(compActivo, compInactivo);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetByMaquinariaIdDtoAsync("M1", includeInactive: false);

        // Assert
        result.Should().HaveCount(1);
        result.First().IdComponente.Should().Be("ACT");
    }

    [Fact]
    public async Task GetByMaquinariaIdDtoAsync_Should_Return_Empty_For_NonExistent_Maquinaria()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new ComponenteService(repository, context);

        // Act
        var result = await service.GetByMaquinariaIdDtoAsync("NON-EXISTENT");

        // Assert
        result.Should().BeEmpty();
    }
}
