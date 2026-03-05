using Microsoft.EntityFrameworkCore;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;
using Orión.Domain.Entities;

namespace Orión.Application.Services;

public class ComponenteService : IComponenteService
{
    private readonly IRepository<Componente> _repository;
    private readonly IOrionDbContext _context;

    public ComponenteService(IRepository<Componente> repository, IOrionDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<IEnumerable<ComponenteDto>> GetByMaquinariaIdDtoAsync(string maquinariaId, bool includeInactive = false)
    {
        var query = _repository.GetQueryable()
            .Where(c => c.IdMaquinaria == maquinariaId)
            .Include(c => c.TipoComponente)
            .Include(c => c.EstadoComponente)
            .Include(c => c.Proveedor)
            .AsQueryable();

        if (!includeInactive)
        {
            query = query.Where(c => c.Activo);
        }

        var list = await query.ToListAsync();

        return list.Select(c => new ComponenteDto
        {
            IdComponente = c.IdComponente,
            NombreComponente = c.NombreComponente,
            Marca = c.Marca,
            NumeroSerie = c.NumeroSerie,
            EspecificacionesTecnicas = c.EspecificacionesTecnicas,
            FechaUltimoCambio = c.FechaUltimoCambio,
            IdMaquinaria = c.IdMaquinaria,
            IdTipoComponente = c.IdTipoComponente,
            TipoComponenteNombre = c.TipoComponente.NombreTipo,
            IdEstado = c.IdEstado,
            EstadoDescripcion = c.EstadoComponente.DescripcionEstado,
            IdProveedor = c.IdProveedor,
            ProveedorNombre = c.Proveedor?.Nombre,
            Activo = c.Activo
        });
    }

    public async Task<IEnumerable<Componente>> GetByMaquinariaIdAsync(string maquinariaId)
    {
        return await _repository.GetQueryable()
            .Where(c => c.IdMaquinaria == maquinariaId && c.Activo)
            .ToListAsync();
    }

    public async Task<Componente?> GetByIdAsync(string id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateAsync(Componente componente)
    {
        await _repository.AddAsync(componente);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Componente componente)
    {
        _repository.Update(componente);
        await _context.SaveChangesAsync();
    }

    public async Task ToggleStatusAsync(string id)
    {
        var componente = await _repository.GetByIdAsync(id);
        if (componente != null)
        {
            componente.Activo = !componente.Activo;
            _repository.Update(componente);
            await _context.SaveChangesAsync();
        }
    }
}
