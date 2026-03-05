using Microsoft.EntityFrameworkCore;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;
using Orión.Domain.Entities;

namespace Orión.Application.Services;

public class ProveedorService : IProveedorService
{
    private readonly IRepository<Proveedor> _repository;
    private readonly IOrionDbContext _context;

    public ProveedorService(IRepository<Proveedor> repository, IOrionDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<IEnumerable<ProveedorDto>> GetAllDtoAsync(bool includeInactive = false)
    {
        var query = _repository.GetQueryable();

        if (!includeInactive)
        {
            query = query.Where(p => p.Activo);
        }

        var list = await query.ToListAsync();

        return list.Select(p => new ProveedorDto
        {
            IdProveedor = p.IdProveedor,
            Nombre = p.Nombre,
            RUC = p.RUC,
            Telefono = p.Telefono,
            Email = p.Email,
            Direccion = p.Direccion,
            Activo = p.Activo
        });
    }

    public async Task<IEnumerable<Proveedor>> GetAllAsync()
    {
        return await _repository.GetQueryable()
            .Where(p => p.Activo)
            .ToListAsync();
    }

    public async Task<Proveedor?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Proveedor?> GetByNameAsync(string name)
    {
        return await _repository.GetQueryable()
            .FirstOrDefaultAsync(p => p.Nombre.ToLower() == name.ToLower());
    }

    public async Task CreateAsync(Proveedor proveedor)
    {
        await _repository.AddAsync(proveedor);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Proveedor proveedor)
    {
        _repository.Update(proveedor);
        await _context.SaveChangesAsync();
    }

    public async Task ToggleStatusAsync(int id)
    {
        var proveedor = await _repository.GetByIdAsync(id);
        if (proveedor != null)
        {
            proveedor.Activo = !proveedor.Activo;
            _repository.Update(proveedor);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Proveedor> GetOrCreateByNameAsync(string name)
    {
        var existing = await GetByNameAsync(name);
        if (existing != null) return existing;

        var nuevo = new Proveedor { Nombre = name, Activo = true };
        await _repository.AddAsync(nuevo);
        await _context.SaveChangesAsync();
        return nuevo;
    }
}
