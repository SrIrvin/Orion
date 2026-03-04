using Orión.Application.DTOs;
using Orión.Domain.Entities;

namespace Orión.Application.Interfaces;

public interface IMaquinariaService
{
    Task<IEnumerable<MaquinariaDto>> GetAllDtoAsync(bool includeInactive = false);
    Task<IEnumerable<Maquinaria>> GetAllAsync();
    Task<Maquinaria?> GetByIdAsync(string id);
    Task CreateAsync(Maquinaria maquinaria);
    Task UpdateAsync(Maquinaria maquinaria);
    Task ToggleStatusAsync(string id);
}
