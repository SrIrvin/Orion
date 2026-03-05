using Microsoft.EntityFrameworkCore;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;
using Orión.Domain.Entities;

namespace Orión.Application.Services;

public class TecnicoService : ITecnicoService
{
    private readonly IRepository<Tecnico> _repository;
    private readonly IOrionDbContext _context;

    public TecnicoService(IRepository<Tecnico> repository, IOrionDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<IEnumerable<TecnicoDto>> GetAllDtoAsync(bool includeInactive = false)
    {
        var query = _repository.GetQueryable()
            .Include(t => t.Turno)
            .AsQueryable();

        if (!includeInactive)
        {
            query = query.Where(t => t.Activo);
        }

        var list = await query.ToListAsync();

        return list.Select(t => new TecnicoDto
        {
            IdPersonal = t.IdPersonal,
            NombreApellido = t.NombreApellido,
            Especialidad = t.Especialidad,
            IdTurno = t.IdTurno,
            TurnoDescripcion = t.Turno.DescripcionTurno,
            Activo = t.Activo
        });
    }

    public async Task<IEnumerable<Tecnico>> GetAllAsync()
    {
        return await _repository.GetQueryable()
            .Where(t => t.Activo)
            .ToListAsync();
    }

    public async Task<Tecnico?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateAsync(Tecnico tecnico)
    {
        await _repository.AddAsync(tecnico);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tecnico tecnico)
    {
        _repository.Update(tecnico);
        await _context.SaveChangesAsync();
    }

    public async Task ToggleStatusAsync(int id)
    {
        var tecnico = await _repository.GetByIdAsync(id);
        if (tecnico != null)
        {
            tecnico.Activo = !tecnico.Activo;
            _repository.Update(tecnico);
            await _context.SaveChangesAsync();
        }
    }
}
