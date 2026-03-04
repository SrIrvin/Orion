using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Orión.Application.Interfaces;
using Orión.Application.Services;
using Orión.Domain.Entities;
using Orión.Infrastructure.Persistence;
using Orión.Infrastructure.Repositories;
using Xunit;

namespace Orión.Tests.Application;

public class TecnicoServiceTests
{
    private (IOrionDbContext context, IRepository<Tecnico> repository) GetDependencies()
    {
        var options = new DbContextOptionsBuilder<OrionDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new OrionDbContext(options);
        var repository = new GenericRepository<Tecnico>(context);
        return (context, repository);
    }

    [Fact]
    public async Task GetAllDtoAsync_Should_Return_Mapped_Tecnicos_With_Turnos()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new TecnicoService(repository, context);

        var turno = new Turno { IdTurno = 1, DescripcionTurno = "Matutino" };
        var tecnico = new Tecnico 
        { 
            IdPersonal = 101, 
            NombreApellido = "Juan Perez", 
            Especialidad = "Mecánico", 
            Turno = turno 
        };

        await context.Set<Tecnico>().AddAsync(tecnico);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetAllDtoAsync(includeInactive: true);

        // Assert
        result.Should().HaveCount(1);
        var dto = result.First();
        dto.NombreApellido.Should().Be("Juan Perez");
        dto.TurnoDescripcion.Should().Be("Matutino");
    }

    [Fact]
    public async Task GetAllDtoAsync_Should_Exclude_Inactive_When_Flag_Is_False()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new TecnicoService(repository, context);

        var turno = new Turno { IdTurno = 1, DescripcionTurno = "Matutino" };
        var tecnicos = new List<Tecnico>
        {
            new Tecnico { IdPersonal = 1, NombreApellido = "Activo", Activo = true, Turno = turno },
            new Tecnico { IdPersonal = 2, NombreApellido = "Inactivo", Activo = false, Turno = turno }
        };

        await context.Set<Tecnico>().AddRangeAsync(tecnicos);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetAllDtoAsync(includeInactive: false);

        // Assert
        result.Should().HaveCount(1);
        result.First().NombreApellido.Should().Be("Activo");
    }

    [Fact]
    public async Task ToggleStatusAsync_Should_Inactivate_Active_Tecnico()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new TecnicoService(repository, context);
        var tecnico = new Tecnico { IdPersonal = 100, NombreApellido = "Test", Activo = true };
        await context.Set<Tecnico>().AddAsync(tecnico);
        await context.SaveChangesAsync();

        // Act
        await service.ToggleStatusAsync(100);
        var result = await service.GetByIdAsync(100);

        // Assert
        result.Should().NotBeNull();
        result!.Activo.Should().BeFalse();
    }

    [Fact]
    public async Task ToggleStatusAsync_Should_Activate_Inactive_Tecnico()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new TecnicoService(repository, context);
        var tecnico = new Tecnico { IdPersonal = 100, NombreApellido = "Test", Activo = false };
        await context.Set<Tecnico>().AddAsync(tecnico);
        await context.SaveChangesAsync();

        // Act
        await service.ToggleStatusAsync(100);
        var result = await service.GetByIdAsync(100);

        // Assert
        result.Should().NotBeNull();
        result!.Activo.Should().BeTrue();
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Correct_Tecnico()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new TecnicoService(repository, context);
        var tecnico = new Tecnico { IdPersonal = 500, NombreApellido = "Test", IdTurno = 1 };
        await context.Set<Tecnico>().AddAsync(tecnico);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetByIdAsync(500);

        // Assert
        result.Should().NotBeNull();
        result!.NombreApellido.Should().Be("Test");
    }
}
