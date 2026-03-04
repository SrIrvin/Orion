using Microsoft.EntityFrameworkCore;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;
using Orión.Domain.Entities;
using BC = BCrypt.Net.BCrypt;

namespace Orión.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IRepository<Usuario> _repository;
    private readonly IOrionDbContext _context;

    public UsuarioService(IRepository<Usuario> repository, IOrionDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
    {
        var usuarios = await _repository.GetAllAsync();
        return usuarios.Select(u => new UsuarioDto
        {
            IdUsuario = u.IdUsuario,
            NombreUsuario = u.NombreUsuario,
            Email = u.Email,
            Rol = u.Rol,
            Activo = u.Activo,
            FechaCreacion = u.FechaCreacion
        });
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateAsync(Usuario usuario, string password)
    {
        usuario.PasswordHash = BC.HashPassword(password);
        usuario.FechaCreacion = DateTime.UtcNow;
        usuario.Activo = true;

        await _repository.AddAsync(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Usuario usuario)
    {
        _repository.Update(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task ToggleStatusAsync(int id)
    {
        var usuario = await _repository.GetByIdAsync(id);
        if (usuario != null)
        {
            usuario.Activo = !usuario.Activo;
            _repository.Update(usuario);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ResetPasswordAsync(int id, string newPassword)
    {
        var usuario = await _repository.GetByIdAsync(id);
        if (usuario != null)
        {
            usuario.PasswordHash = BC.HashPassword(newPassword);
            _repository.Update(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
