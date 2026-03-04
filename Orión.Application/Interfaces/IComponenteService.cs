using Orión.Application.DTOs;
using Orión.Domain.Entities;

namespace Orión.Application.Interfaces;

public interface IComponenteService
{
    Task<IEnumerable<ComponenteDto>> GetByMaquinariaIdDtoAsync(string maquinariaId);
    Task<IEnumerable<Componente>> GetByMaquinariaIdAsync(string maquinariaId);
    Task<Componente?> GetByIdAsync(string id);
    Task CreateAsync(Componente componente);
    Task UpdateAsync(Componente componente);
    Task DeleteAsync(string id);
}
