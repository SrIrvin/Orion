using Orión.Application.DTOs;
using Orión.Domain.Entities;

namespace Orión.Application.Interfaces;

public interface ISolicitudServicioService
{
    Task<IEnumerable<SolicitudServicioDto>> GetAllDtoAsync();
    Task<IEnumerable<SolicitudServicioDto>> GetByMaquinariaDtoAsync(string maquinariaId);
    Task<SolicitudServicio?> GetByIdAsync(int id);
    Task CreateAsync(SolicitudServicio solicitud);
    Task UpdateStatusAsync(int id, int nuevoEstadoId);
    Task AssignTechnicianAsync(int id, int personalId);
    Task CloseRequestAsync(int id);
}
