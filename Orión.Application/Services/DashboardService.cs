using Microsoft.EntityFrameworkCore;
using Orión.Application.Interfaces;

namespace Orión.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IOrionDbContext _context;

    public DashboardService(IOrionDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetTotalMaquinariaAsync()
    {
        return await _context.Maquinarias.CountAsync();
    }

    public async Task<int> GetTotalTecnicosAsync()
    {
        return await _context.Tecnicos.CountAsync();
    }

    public async Task<int> GetSolicitudesAbiertasAsync()
    {
        return await _context.SolicitudesServicios.CountAsync(s => s.IdEstadoSolicitud == 1);
    }
}
