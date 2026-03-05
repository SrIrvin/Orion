using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Orión.Application.Interfaces;
using Orión.Application.Services;
using Orión.Domain.Entities;
using Orión.Infrastructure.Persistence;
using Orión.Infrastructure.Repositories;
using Xunit;

namespace Orión.Tests.Application;

public class MaquinariaServiceTests
{
    private (IOrionDbContext context, IRepository<Maquinaria> repository) GetDependencies()
    {
        var options = new DbContextOptionsBuilder<OrionDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new OrionDbContext(options);
        var repository = new GenericRepository<Maquinaria>(context);
        return (context, repository);
    }

    [Fact]
    public async Task GetAllDtoAsync_Should_Return_Mapped_Dtos()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new MaquinariaService(repository, context);

        var nivel = new NivelCritico { IdNivelCritico = 1, Descripcion = "Alta" };
        var ubicacion = new Ubicacion { IdUbicacion = 1, NumeroNave = 5 };
        
        var maquina = new Maquinaria 
        { 
            IdMaquinaria = "M1", 
            NombreMaquina = "Prensa", 
            NivelCritico = nivel, 
            Ubicacion = ubicacion 
        };

        await context.Set<Maquinaria>().AddAsync(maquina);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetAllDtoAsync();

        // Assert
        result.Should().HaveCount(1);
        var dto = result.First();
        dto.NombreMaquina.Should().Be("Prensa");
        dto.NivelCriticoDescripcion.Should().Be("Alta");
        dto.UbicacionNave.Should().Be("Nave 5");
    }

    [Fact]
    public async Task CreateAsync_Should_Persist_Maquinaria()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new MaquinariaService(repository, context);
        var maquina = new Maquinaria { IdMaquinaria = "NEW-01", NombreMaquina = "Nueva Maquina" };

        // Act
        await service.CreateAsync(maquina);

        // Assert
        var result = await context.Set<Maquinaria>().FindAsync("NEW-01");
        result.Should().NotBeNull();
        result!.NombreMaquina.Should().Be("Nueva Maquina");
    }

    [Fact]
    public async Task GetAllDtoAsync_Should_Filter_Inactive_By_Default()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new MaquinariaService(repository, context);

        var nivel = new NivelCritico { IdNivelCritico = 1, Descripcion = "Alta" };
        var ubicacion = new Ubicacion { IdUbicacion = 1, NumeroNave = 5 };
        
        var maqActiva = new Maquinaria { IdMaquinaria = "ACT-01", NombreMaquina = "Activa", Activo = true, NivelCritico = nivel, Ubicacion = ubicacion };
        var maqInactiva = new Maquinaria { IdMaquinaria = "INA-01", NombreMaquina = "Inactiva", Activo = false, NivelCritico = nivel, Ubicacion = ubicacion };

        await context.Set<Maquinaria>().AddRangeAsync(maqActiva, maqInactiva);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetAllDtoAsync(includeInactive: false);

        // Assert
        result.Should().HaveCount(1);
        result.First().IdMaquinaria.Should().Be("ACT-01");
    }

    [Fact]
    public async Task GetAllDtoAsync_Should_Include_Inactive_When_Requested()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new MaquinariaService(repository, context);

        var nivel = new NivelCritico { IdNivelCritico = 1, Descripcion = "Alta" };
        var ubicacion = new Ubicacion { IdUbicacion = 1, NumeroNave = 5 };
        
        var maqActiva = new Maquinaria { IdMaquinaria = "ACT-01", NombreMaquina = "Activa", Activo = true, NivelCritico = nivel, Ubicacion = ubicacion };
        var maqInactiva = new Maquinaria { IdMaquinaria = "INA-01", NombreMaquina = "Inactiva", Activo = false, NivelCritico = nivel, Ubicacion = ubicacion };

        await context.Set<Maquinaria>().AddRangeAsync(maqActiva, maqInactiva);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetAllDtoAsync(includeInactive: true);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_For_Invalid_Id()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new MaquinariaService(repository, context);

        // Act
        var result = await service.GetByIdAsync("INVALID-ID");

        // Assert
        result.Should().BeNull();
    }
}
