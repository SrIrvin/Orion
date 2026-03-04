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
}
