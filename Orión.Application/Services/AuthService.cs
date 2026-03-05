using Microsoft.EntityFrameworkCore;
using Orión.Application.Interfaces;
using Orión.Domain.Entities;
using Orión.Domain.Exceptions;
using BC = BCrypt.Net.BCrypt;

namespace Orión.Application.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<Usuario> _repository;
    private readonly IOrionDbContext _context;

    public AuthService(IRepository<Usuario> repository, IOrionDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<Usuario?> LoginAsync(string username, string password)
    {
        var query = await _repository.FindAsync(u => u.NombreUsuario == username && u.Activo);
        var usuario = query.FirstOrDefault();

        if (usuario == null || !BC.Verify(password, usuario.PasswordHash))
        {
            return null;
        }

        return usuario;
    }

    public async Task<Usuario> RegisterAsync(string username, string password, string? email, string rol)
    {
        // Verificar si el usuario ya existe
        var query = await _repository.FindAsync(u => u.NombreUsuario == username);
        if (query.Any())
        {
            throw new UserAlreadyExistsException(username);
        }

        var usuario = new Usuario
        {
            NombreUsuario = username,
            PasswordHash = BC.HashPassword(password),
            Email = email,
            Rol = rol,
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };

        await _repository.AddAsync(usuario);
        await _context.SaveChangesAsync();

        return usuario;
    }
}
