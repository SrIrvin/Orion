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

        // 1. Crear usuario administrador por defecto
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
        }

        // 2. Asegurar Datos Maestros para Maquinaria (Evitar Violación de FK)
        if (!context.NivelesCriticos.Any())
        {
            context.NivelesCriticos.AddRange(
                new NivelCritico { IdNivelCritico = 1, Descripcion = "Baja" },
                new NivelCritico { IdNivelCritico = 2, Descripcion = "Media" },
                new NivelCritico { IdNivelCritico = 3, Descripcion = "Alta" },
                new NivelCritico { IdNivelCritico = 4, Descripcion = "Critico" }
            );
        }

        if (!context.Ubicaciones.Any())
        {
            context.Ubicaciones.Add(new Ubicacion { NumeroNave = 1 });
        }

        context.SaveChanges(); // Guardar catálogos primero

        // 3. Datos de prueba para Maquinaria
        if (!context.Maquinarias.Any())
        {
            var nivelAlta = context.NivelesCriticos.First(n => n.IdNivelCritico == 3);
            var ubicacionNave1 = context.Ubicaciones.First(u => u.NumeroNave == 1);

            context.Maquinarias.Add(new Maquinaria
            {
                IdMaquinaria = "MAQ-001",
                NombreMaquina = "Prensa Hidr 50T",
                Tipo = "Prensa",
                Marca = "HydraForce",
                Modelo = "HF-50",
                FechaInstalacion = DateTime.UtcNow.AddYears(-2),
                IdNivelCritico = nivelAlta.IdNivelCritico,
                IdUbicacion = ubicacionNave1.IdUbicacion
            });

            context.Maquinarias.Add(new Maquinaria
            {
                IdMaquinaria = "MAQ-002",
                NombreMaquina = "Torno CNC Quick",
                Tipo = "Torno",
                Marca = "Mazak",
                Modelo = "Quick Turn 250",
                FechaInstalacion = DateTime.UtcNow.AddYears(-1),
                IdNivelCritico = 4,
                IdUbicacion = ubicacionNave1.IdUbicacion
            });
            
            context.SaveChanges();
        }
    }
}
