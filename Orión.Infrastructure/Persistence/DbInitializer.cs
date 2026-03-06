using Microsoft.EntityFrameworkCore;
using Orión.Domain.Entities;
using BC = BCrypt.Net.BCrypt;

namespace Orión.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Initialize(OrionDbContext context)
    {
        // Asegura que la base de datos existe y se prepara según el proveedor
        if (context.Database.IsNpgsql())
        {
            context.Database.Migrate();
        }
        else
        {
            // Para Access u otros, EnsureCreated es más seguro si no hay migraciones específicas
            context.Database.EnsureCreated();
        }

        // 0. Asegurar Catálogos Básicos (Ubicaciones y Tipos de Componentes)
        if (!context.Ubicaciones.Any())
        {
            context.Ubicaciones.Add(new Ubicacion { NumeroNave = 1 });
            context.SaveChanges();
        }

        if (!context.TiposComponentes.Any())
        {
            context.TiposComponentes.AddRange(
                new TipoComponente { NombreTipo = "Motor" },
                new TipoComponente { NombreTipo = "Bomba" },
                new TipoComponente { NombreTipo = "Sensor" },
                new TipoComponente { NombreTipo = "Valvula" }
            );
            context.SaveChanges();
        }

        // 1. Crear usuarios por defecto
        if (!context.Usuarios.Any(u => u.NombreUsuario == "admin"))
        {
            context.Usuarios.Add(new Usuario
            {
                NombreUsuario = "admin",
                PasswordHash = BC.HashPassword("admin123"),
                Email = "admin@orion.com",
                Rol = "Admin",
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
                Activo = true
            });
        }
        context.SaveChanges();

        // 2. Datos de prueba para Maquinaria
        if (!context.Maquinarias.Any())
        {
            var ubicacionId = context.Ubicaciones.First().IdUbicacion;

            context.Maquinarias.AddRange(
                new Maquinaria
                {
                    IdMaquinaria = "MAQ-001",
                    NombreMaquina = "Prensa Hidráulica 50T",
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
                    NombreMaquina = "Torno CNC Quick Turn",
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

        // 3. Datos de prueba para Personal Técnico
        if (!context.Tecnicos.Any())
        {
            context.Tecnicos.AddRange(
                new Tecnico { IdPersonal = 101, NombreApellido = "Sr. Juan Pérez", Especialidad = "Mecánico", IdTurno = 1, Activo = true },
                new Tecnico { IdPersonal = 102, NombreApellido = "Ing. Maria García", Especialidad = "Electrónica", IdTurno = 2, Activo = true },
                new Tecnico { IdPersonal = 103, NombreApellido = "Pedro López", Especialidad = "Soldador", IdTurno = 3, Activo = true }
            );
            context.SaveChanges();
        }

        // 4. Datos de prueba para Componentes
        if (!context.Componentes.Any())
        {
            var maq = context.Maquinarias.First();
            var tipo = context.TiposComponentes.First();

            context.Componentes.Add(new Componente
            {
                IdComponente = $"COMP-{maq.IdMaquinaria}-01",
                IdMaquinaria = maq.IdMaquinaria,
                NombreComponente = "Motor Principal de Inducción",
                IdTipoComponente = tipo.IdTipoComponente,
                Marca = "Siemens",
                NumeroSerie = $"SN-{maq.IdMaquinaria}-MP",
                IdEstado = 1,
                Activo = true
            });
            context.SaveChanges();
        }
    }
}
