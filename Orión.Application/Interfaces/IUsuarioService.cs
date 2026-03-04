using Orión.Application.DTOs;
using Orión.Domain.Entities;

namespace Orión.Application.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDto>> GetAllAsync();
    Task<Usuario?> GetByIdAsync(int id);
    Task CreateAsync(Usuario usuario, string password);
    Task UpdateAsync(Usuario usuario);
    Task ToggleStatusAsync(int id);
    Task ResetPasswordAsync(int id, string newPassword);
}
