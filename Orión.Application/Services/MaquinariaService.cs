using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<MaquinariaDto>> GetAllDtoAsync(bool includeInactive = false)
    {
        var query = _repository.GetQueryable()
            .Include(m => m.NivelCritico)
            .Include(m => m.Ubicacion)
            .AsQueryable();

        if (!includeInactive)
        {
            query = query.Where(m => m.Activo);
        }

        var list = await query.ToListAsync();

        return list.Select(m => new MaquinariaDto
        {
            IdMaquinaria = m.IdMaquinaria,
            NombreMaquina = m.NombreMaquina,
            Tipo = m.Tipo,
            Marca = m.Marca,
            Modelo = m.Modelo,
            FechaInstalacion = m.FechaInstalacion,
            IdNivelCritico = m.IdNivelCritico,
            NivelCriticoDescripcion = m.NivelCritico.Descripcion,
            IdUbicacion = m.IdUbicacion,
            UbicacionNave = $"Nave {m.Ubicacion.NumeroNave}",
            Activo = m.Activo
        });
    }

    public async Task<IEnumerable<Maquinaria>> GetAllAsync()
    {
        return await _repository.GetQueryable()
            .Where(m => m.Activo)
            .ToListAsync();
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

    public async Task ToggleStatusAsync(string id)
    {
        var maquinaria = await _repository.GetByIdAsync(id);
        if (maquinaria != null)
        {
            maquinaria.Activo = !maquinaria.Activo;
            _repository.Update(maquinaria);
            await _context.SaveChangesAsync();
        }
    }
}
