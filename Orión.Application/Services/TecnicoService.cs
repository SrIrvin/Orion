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

    public async Task<IEnumerable<TecnicoDto>> GetAllDtoAsync()
    {
        var tecnicos = await _repository.GetAllWithIncludesAsync(t => t.Turno);

        return tecnicos.Select(t => new TecnicoDto
        {
            IdPersonal = t.IdPersonal,
            NombreApellido = t.NombreApellido,
            Especialidad = t.Especialidad,
            TurnoDescripcion = t.Turno.DescripcionTurno
        });
    }

    public async Task<IEnumerable<Tecnico>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
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

    public async Task DeleteAsync(int id)
    {
        var tecnico = await _repository.GetByIdAsync(id);
        if (tecnico != null)
        {
            _repository.Remove(tecnico);
            await _context.SaveChangesAsync();
        }
    }
}
