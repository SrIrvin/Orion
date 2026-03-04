using Orión.Application.DTOs;
using Orión.Domain.Entities;

namespace Orión.Application.Interfaces;

public interface IComponenteService
{
    Task<IEnumerable<ComponenteDto>> GetByMaquinariaIdDtoAsync(string maquinariaId, bool includeInactive = false);
    Task<IEnumerable<Componente>> GetByMaquinariaIdAsync(string maquinariaId);
    Task<Componente?> GetByIdAsync(string id);
    Task CreateAsync(Componente componente);
    Task UpdateAsync(Componente componente);
    Task ToggleStatusAsync(string id);
}
