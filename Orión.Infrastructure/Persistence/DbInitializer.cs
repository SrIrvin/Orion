using Microsoft.EntityFrameworkCore;
using Orión.Domain.Entities;
using BC = BCrypt.Net.BCrypt;

namespace Orión.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Initialize(OrionDbContext context)
    {
        // Asegura que la base de datos existe y aplica todas las migraciones pendientes
        context.Database.Migrate();

        // Crear usuario administrador por defecto si no existe ninguno
        if (!context.Usuarios.Any())
        {
            context.Usuarios.Add(new Usuario
            {
                NombreUsuario = "admin",
                PasswordHash = BC.HashPassword("admin123"),
                Email = "admin@orion.com",
                Rol = "Admin",
                FechaCreacion = DateTime.UtcNow,
                Activo = true
            });
            context.SaveChanges();
        }
    }
}
