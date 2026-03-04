using Microsoft.EntityFrameworkCore;
using Orión.Domain.Entities;

namespace Orión.Application.Interfaces;

public interface IOrionDbContext
{
    DbSet<Maquinaria> Maquinarias { get; set; }
    DbSet<NivelCritico> NivelesCriticos { get; set; }
    DbSet<Ubicacion> Ubicaciones { get; set; }
    DbSet<Componente> Componentes { get; set; }
    DbSet<TipoComponente> TiposComponentes { get; set; }
    DbSet<EstadoComponente> EstadosComponentes { get; set; }
    DbSet<Tecnico> Tecnicos { get; set; }
    DbSet<Turno> Turnos { get; set; }
    DbSet<SolicitudServicio> SolicitudesServicios { get; set; }
    DbSet<TipoMantenimiento> TiposMantenimiento { get; set; }
    DbSet<EstadoSolicitud> EstadosSolicitudes { get; set; }
    DbSet<Usuario> Usuarios { get; set; }

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
