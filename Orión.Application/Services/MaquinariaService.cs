using Orión.Application.DTOs;
using Orión.Application.Interfaces;
using Orión.Domain.Entities;

namespace Orión.Application.Services;

public class MaquinariaService : IMaquinariaService
{
    private readonly IRepository<Maquinaria> _repository;
    private readonly IOrionDbContext _context;

    public MaquinariaService(IRepository<Maquinaria> repository, IOrionDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<IEnumerable<MaquinariaDto>> GetAllDtoAsync()
    {
        var maquinas = await _repository.GetAllWithIncludesAsync(
            m => m.NivelCritico,
            m => m.Ubicacion);

        return maquinas.Select(m => new MaquinariaDto
        {
            IdMaquinaria = m.IdMaquinaria,
            NombreMaquina = m.NombreMaquina,
            Tipo = m.Tipo,
            Marca = m.Marca,
            Modelo = m.Modelo,
            FechaInstalacion = m.FechaInstalacion,
            NivelCriticoDescripcion = m.NivelCritico.Descripcion,
            UbicacionNave = $"Nave {m.Ubicacion.NumeroNave}"
        });
    }

    public async Task<IEnumerable<Maquinaria>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Maquinaria?> GetByIdAsync(string id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateAsync(Maquinaria maquinaria)
    {
        await _repository.AddAsync(maquinaria);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Maquinaria maquinaria)
    {
        _repository.Update(maquinaria);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var maquinaria = await _repository.GetByIdAsync(id);
        if (maquinaria != null)
        {
            _repository.Remove(maquinaria);
            await _context.SaveChangesAsync();
        }
    }
}
