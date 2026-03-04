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

    public async Task<IEnumerable<ComponenteDto>> GetByMaquinariaIdDtoAsync(string maquinariaId)
    {
        var componentes = await _repository.GetWithIncludesAsync(
            c => c.IdMaquinaria == maquinariaId,
            c => c.TipoComponente,
            c => c.EstadoComponente);

        return componentes.Select(c => new ComponenteDto
        {
            IdComponente = c.IdComponente,
            NombreComponente = c.NombreComponente,
            Marca = c.Marca,
            NumeroSerie = c.NumeroSerie,
            EspecificacionesTecnicas = c.EspecificacionesTecnicas,
            FechaUltimoCambio = c.FechaUltimoCambio,
            IdMaquinaria = c.IdMaquinaria,
            TipoComponenteNombre = c.TipoComponente.NombreTipo,
            EstadoDescripcion = c.EstadoComponente.DescripcionEstado
        });
    }

    public async Task<IEnumerable<Componente>> GetByMaquinariaIdAsync(string maquinariaId)
    {
        return await _repository.FindAsync(c => c.IdMaquinaria == maquinariaId);
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

    public async Task DeleteAsync(string id)
    {
        var componente = await _repository.GetByIdAsync(id);
        if (componente != null)
        {
            _repository.Remove(componente);
            await _context.SaveChangesAsync();
        }
    }
}
