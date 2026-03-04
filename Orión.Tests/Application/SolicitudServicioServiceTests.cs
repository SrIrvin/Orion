using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Orión.Application.Interfaces;
using Orión.Application.Services;
using Orión.Domain.Entities;
using Orión.Infrastructure.Persistence;
using Orión.Infrastructure.Repositories;
using Xunit;

namespace Orión.Tests.Application;

public class SolicitudServicioServiceTests
{
    private (IOrionDbContext context, IRepository<SolicitudServicio> repository) GetDependencies()
    {
        var options = new DbContextOptionsBuilder<OrionDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new OrionDbContext(options);
        var repository = new GenericRepository<SolicitudServicio>(context);
        return (context, repository);
    }

    [Fact]
    public async Task CreateAsync_Should_Set_Initial_State_And_Date()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new SolicitudServicioService(repository, context);
        
        var solicitud = new SolicitudServicio 
        { 
            IdMaquinaria = "MAQ1", 
            IdTipoMantto = 1,
            DescripcionFalla = "Test"
        };

        // Act
        await service.CreateAsync(solicitud);

        // Assert
        solicitud.FechaApertura.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        solicitud.IdEstadoSolicitud.Should().Be(1); // Abierta
    }

    [Fact]
    public async Task GetAllDtoAsync_Should_Map_Related_Entities()
    {
        // Arrange
        var (context, repository) = GetDependencies();
        var service = new SolicitudServicioService(repository, context);

        var maquina = new Maquinaria { IdMaquinaria = "M1", NombreMaquina = "Prensa", IdNivelCritico = 1, IdUbicacion = 1 };
        var tipo = new TipoMantenimiento { IdTipoMantto = 1, DescripcionTipo = "Preventivo" };
        var estado = new EstadoSolicitud { IdEstadoSolicitud = 1, DescripcionEstado = "Abierta" };
        
        var solicitud = new SolicitudServicio 
        { 
            IdSS = 1, 
            IdMaquinaria = "M1", 
            IdTipoMantto = 1, 
            IdEstadoSolicitud = 1,
            Maquinaria = maquina,
            TipoMantenimiento = tipo,
            EstadoSolicitud = estado
        };

        await context.Set<SolicitudServicio>().AddAsync(solicitud);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetAllDtoAsync();

        // Assert
        result.Should().HaveCount(1);
        var dto = result.First();
        dto.NombreMaquinaria.Should().Be("Prensa");
        dto.TipoMantenimientoDescripcion.Should().Be("Preventivo");
        dto.EstadoDescripcion.Should().Be("Abierta");
    }
}
