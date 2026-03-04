using Microsoft.EntityFrameworkCore;
using Orión.Application.Interfaces;
using Orión.Domain.Entities;
using Orión.Domain.Exceptions;
using BC = BCrypt.Net.BCrypt;

namespace Orión.Application.Services;

public class AuthService : IAuthService
{
    private readonly IOrionDbContext _context;

    public AuthService(IOrionDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> LoginAsync(string username, string password)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.NombreUsuario == username && u.Activo);

        if (usuario == null || !BC.Verify(password, usuario.PasswordHash))
        {
            return null;
        }

        return usuario;
    }

    public async Task<Usuario> RegisterAsync(string username, string password, string? email, string rol)
    {
        // Verificar si el usuario ya existe
        if (await _context.Usuarios.AnyAsync(u => u.NombreUsuario == username))
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

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return usuario;
    }
}
