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
}
