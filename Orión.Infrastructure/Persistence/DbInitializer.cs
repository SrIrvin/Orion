using Microsoft.EntityFrameworkCore;
using Orión.Domain.Entities;
using BC = BCrypt.Net.BCrypt;

namespace Orión.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Initialize(OrionDbContext context)
    {
        // Asegura que la base de datos existe y aplica todas las migraciones pendientes de forma nativa
        context.Database.Migrate();

        // 1. Crear usuarios por defecto
        if (!context.Usuarios.Any(u => u.NombreUsuario == "admin"))
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

        if (!context.Usuarios.Any(u => u.NombreUsuario == "operador"))
        {
            context.Usuarios.Add(new Usuario
            {
                NombreUsuario = "operador",
                PasswordHash = BC.HashPassword("user123"),
                Email = "operador@orion.com",
                Rol = "Operador",
                FechaCreacion = DateTime.UtcNow,
                Activo = true
            });
        }
        context.SaveChanges();

        // 2. Asegurar Ubicaciones (Nave 1 es fundamental para el seed)
        if (!context.Ubicaciones.Any(u => u.NumeroNave == 1))
        {
            context.Ubicaciones.Add(new Ubicacion { NumeroNave = 1 });
            context.SaveChanges();
        }

        // 3. Datos de prueba para Maquinaria
        if (!context.Maquinarias.Any())
        {
            var ubicacionId = context.Ubicaciones.First(u => u.NumeroNave == 1).IdUbicacion;

            context.Maquinarias.AddRange(
                new Maquinaria
                {
                    IdMaquinaria = "MAQ-001",
                    NombreMaquina = "Prensa Hidr 50T",
                    Tipo = "Prensa",
                    Marca = "HydraForce",
                    Modelo = "HF-50",
                    FechaInstalacion = DateTime.UtcNow.AddYears(-2),
                    IdNivelCritico = 3, // Alta
                    IdUbicacion = ubicacionId
                },
                new Maquinaria
                {
                    IdMaquinaria = "MAQ-002",
                    NombreMaquina = "Torno CNC Quick",
                    Tipo = "Torno",
                    Marca = "Mazak",
                    Modelo = "Quick Turn 250",
                    FechaInstalacion = DateTime.UtcNow.AddYears(-1),
                    IdNivelCritico = 4, // Critico
                    IdUbicacion = ubicacionId
                }
            );
            context.SaveChanges();
        }

        // 4. Datos de prueba para Personal Técnico
        if (!context.Tecnicos.Any())
        {
            context.Tecnicos.AddRange(
                new Tecnico { IdPersonal = 101, NombreApellido = "Sr. Juan Pérez", Especialidad = "Mecánico", IdTurno = 1, Activo = true },
                new Tecnico { IdPersonal = 102, NombreApellido = "Ing. Maria García", Especialidad = "Electrónica", IdTurno = 2, Activo = true },
                new Tecnico { IdPersonal = 103, NombreApellido = "Pedro López", Especialidad = "Soldador", IdTurno = 3, Activo = true }
            );
            context.SaveChanges();
        }
    }
}
