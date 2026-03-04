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
        var result = await service.GetAllDtoAsync();

        // Assert
        result.Should().HaveCount(1);
        var dto = result.First();
        dto.NombreApellido.Should().Be("Juan Perez");
        dto.TurnoDescripcion.Should().Be("Matutino");
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
