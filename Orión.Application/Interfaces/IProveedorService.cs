using Orión.Application.DTOs;
using Orión.Domain.Entities;

namespace Orión.Application.Interfaces;

public interface IProveedorService
{
    Task<IEnumerable<Proveedor>> GetAllAsync();
    Task<IEnumerable<ProveedorDto>> GetAllDtoAsync(bool includeInactive = false);
    Task<Proveedor?> GetByIdAsync(int id);
    Task<Proveedor?> GetByNameAsync(string name);
    Task CreateAsync(Proveedor proveedor);
    Task UpdateAsync(Proveedor proveedor);
    Task ToggleStatusAsync(int id);
    Task<Proveedor> GetOrCreateByNameAsync(string name);
}
