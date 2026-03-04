using Orión.Application.DTOs;
using Orión.Domain.Entities;

namespace Orión.Application.Interfaces;

public interface ITecnicoService
{
    Task<IEnumerable<TecnicoDto>> GetAllDtoAsync(bool includeInactive = false);
    Task<IEnumerable<Tecnico>> GetAllAsync();
    Task<Tecnico?> GetByIdAsync(int id);
    Task CreateAsync(Tecnico tecnico);
    Task UpdateAsync(Tecnico tecnico);
    Task ToggleStatusAsync(int id);
}
