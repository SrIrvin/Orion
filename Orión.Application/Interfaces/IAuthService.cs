using Orión.Domain.Entities;

namespace Orión.Application.Interfaces;

public interface IAuthService
{
    Task<Usuario?> LoginAsync(string username, string password);
    Task<Usuario> RegisterAsync(string username, string password, string? email, string rol);
}
