using Orión.Application.DTOs;
using Orión.Application.Interfaces;
using Orión.Domain.Entities;

namespace Orión.Application.Services;

public class SolicitudServicioService : ISolicitudServicioService
{
    private readonly IRepository<SolicitudServicio> _repository;
    private readonly IOrionDbContext _context;

    public SolicitudServicioService(IRepository<SolicitudServicio> repository, IOrionDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<IEnumerable<SolicitudServicioDto>> GetAllDtoAsync()
    {
        var solicitudes = await _repository.GetAllWithIncludesAsync(
            s => s.Maquinaria,
            s => s.TipoMantenimiento,
            s => s.Tecnico!,
            s => s.EstadoSolicitud);

        return solicitudes.Select(MapToDto);
    }

    public async Task<IEnumerable<SolicitudServicioDto>> GetByMaquinariaDtoAsync(string maquinariaId)
    {
        var solicitudes = await _repository.GetWithIncludesAsync(
            s => s.IdMaquinaria == maquinariaId,
            s => s.Maquinaria,
            s => s.TipoMantenimiento,
            s => s.Tecnico!,
            s => s.EstadoSolicitud);

        return solicitudes.Select(MapToDto);
    }

    public async Task<SolicitudServicio?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateAsync(SolicitudServicio solicitud)
    {
        solicitud.FechaApertura = DateTime.UtcNow;
        if (solicitud.IdEstadoSolicitud == 0) solicitud.IdEstadoSolicitud = 1; // 1 = Abierta

        await _repository.AddAsync(solicitud);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(int id, int nuevoEstadoId)
    {
        var solicitud = await _repository.GetByIdAsync(id);
        if (solicitud != null)
        {
            solicitud.IdEstadoSolicitud = nuevoEstadoId;
            if (nuevoEstadoId == 4) // 4 = Finalizada
            {
                solicitud.FechaCierre = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
        }
    }

    public async Task AssignTechnicianAsync(int id, int personalId)
    {
        var solicitud = await _repository.GetByIdAsync(id);
        if (solicitud != null)
        {
            solicitud.IdPersonal = personalId;
            solicitud.IdEstadoSolicitud = 2; // 2 = En Proceso
            await _context.SaveChangesAsync();
        }
    }

    public async Task CloseRequestAsync(int id)
    {
        await UpdateStatusAsync(id, 4); // 4 = Finalizada
    }

    private SolicitudServicioDto MapToDto(SolicitudServicio s)
    {
        return new SolicitudServicioDto
        {
            IdSS = s.IdSS,
            DescripcionFalla = s.DescripcionFalla,
            FechaApertura = s.FechaApertura,
            FechaCierre = s.FechaCierre,
            IdMaquinaria = s.IdMaquinaria,
            NombreMaquinaria = s.Maquinaria.NombreMaquina,
            IdTipoMantto = s.IdTipoMantto,
            TipoMantenimientoDescripcion = s.TipoMantenimiento.DescripcionTipo,
            IdPersonal = s.IdPersonal,
            TecnicoNombre = s.Tecnico?.NombreApellido ?? "No asignado",
            IdEstadoSolicitud = s.IdEstadoSolicitud,
            EstadoDescripcion = s.EstadoSolicitud.DescripcionEstado
        };
    }
}
