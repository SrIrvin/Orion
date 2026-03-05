using Orión.Domain.Entities;
using Orión.Infrastructure.Persistence;
using System;
using System.Linq;

namespace Orión.Infrastructure.Persistence;

public static class StagingSeedData
{
    public static void Seed(OrionDbContext context)
    {
        var todasLasMaquinas = context.Maquinarias.ToList();

        // 1. Generar Maquinaria Masiva (20 equipos adicionales)
        if (todasLasMaquinas.Count < 5)
        {
            var ubicacionId = context.Ubicaciones.First().IdUbicacion;
            for (int i = 3; i <= 22; i++)
            {
                context.Maquinarias.Add(new Maquinaria
                {
                    IdMaquinaria = $"STG-MAQ-{i:D3}",
                    NombreMaquina = $"Equipo de Prueba {i}",
                    Tipo = i % 2 == 0 ? "Compresor" : "Motor Industrial",
                    Marca = "Brand-X",
                    Modelo = $"Mod-{i * 100}",
                    FechaInstalacion = DateTime.SpecifyKind(DateTime.UtcNow.AddMonths(-i), DateTimeKind.Utc),
                    IdNivelCritico = (i % 4) + 1,
                    IdUbicacion = ubicacionId,
                    Activo = true
                });
            }
            context.SaveChanges();
            todasLasMaquinas = context.Maquinarias.ToList(); // Recargar después de añadir
        }

        // 2. Generar Técnicos Adicionales
        if (context.Tecnicos.Count() < 5)
        {
            var turnoId = context.Turnos.Any() ? context.Turnos.First().IdTurno : 1;
            for (int i = 104; i <= 110; i++)
            {
                context.Tecnicos.Add(new Tecnico
                {
                    IdPersonal = i,
                    NombreApellido = $"Técnico Especialista {i}",
                    Especialidad = i % 2 == 0 ? "Mecatrónica" : "Hidráulica",
                    IdTurno = turnoId,
                    Activo = true
                });
            }
            context.SaveChanges();
        }

        // 3. Generar Solicitudes de Servicio (Historial masivo para Mapa de Calor)
        if (context.SolicitudesServicios.Count() < 50)
        {
            var maquinasParaSolicitudes = todasLasMaquinas.Take(15).ToList();
            var tecnicos = context.Tecnicos.ToList();
            var random = new Random();

            // Generar datos desde hace 30 días hasta 14 días en el futuro
            for (int i = -30; i <= 14; i++)
            {
                // Determinar cuántas fallas crear para este día (0 a 5 para ver diferentes colores)
                int fallasHoy = random.Next(0, 6); 
                
                for (int f = 0; f < fallasHoy; f++)
                {
                    context.SolicitudesServicios.Add(new SolicitudServicio
                    {
                        IdMaquinaria = maquinasParaSolicitudes[random.Next(maquinasParaSolicitudes.Count)].IdMaquinaria,
                        IdTipoMantto = random.Next(1, 3), // 1: Correctivo, 2: Preventivo
                        DescripcionFalla = $"Reporte de monitoreo automático - Ciclo {i}-{f}",
                        FechaApertura = DateTime.SpecifyKind(DateTime.UtcNow.Date.AddDays(i).AddHours(random.Next(8, 18)), DateTimeKind.Utc),
                        IdPersonal = tecnicos[random.Next(tecnicos.Count)].IdPersonal,
                        IdEstadoSolicitud = i < 0 ? 4 : 1 // 4: Finalizada (pasado), 1: Abierta (presente/futuro)
                    });
                }
            }
            context.SaveChanges();
        }

        // 4. Generar Proveedores (Si no hay suficientes)
        if (context.Proveedores.Count() < 3)
        {
            context.Proveedores.AddRange(
                new Proveedor { Nombre = "Industrial Solutions Corp", RUC = "20123456789", Telefono = "555-0101", Email = "ventas@indsol.com", Direccion = "Av. Industrial 450", Activo = true },
                new Proveedor { Nombre = "Global Machinery Parts", RUC = "20987654321", Telefono = "555-0202", Email = "contact@globalparts.com", Direccion = "Calle Los Repuestos 123", Activo = true },
                new Proveedor { Nombre = "Tech & Tools Logistics", RUC = "20555666777", Telefono = "555-0303", Email = "info@techtools.com", Direccion = "Zona Franca Nave 12", Activo = true }
            );
            context.SaveChanges();
        }

        // 5. Generar Componentes (Asegurar que TODAS las máquinas tengan al menos uno)
        var tipos = context.TiposComponentes.ToList();
        var estados = context.EstadosComponentes.ToList();
        var proveedores = context.Proveedores.ToList();

        if (tipos.Any() && estados.Any())
        {
            int compCounter = 1;
            foreach (var maq in todasLasMaquinas)
            {
                // Solo agregar si la máquina no tiene componentes asignados
                if (!context.Componentes.Any(c => c.IdMaquinaria == maq.IdMaquinaria))
                {
                    int numComp = (compCounter % 3) + 1; // 1 a 3 componentes por máquina
                    for (int i = 1; i <= numComp; i++)
                    {
                        var tipo = tipos[(compCounter + i) % tipos.Count];
                        var proveedor = proveedores.Any() ? proveedores[(compCounter + i) % proveedores.Count] : null;

                        context.Componentes.Add(new Componente
                        {
                            IdComponente = $"C-{maq.IdMaquinaria}-{i:D2}",
                            IdMaquinaria = maq.IdMaquinaria,
                            NombreComponente = $"{tipo.NombreTipo} {i}",
                            IdTipoComponente = tipo.IdTipoComponente,
                            Marca = "OEM-Parts",
                            NumeroSerie = $"SN-{maq.IdMaquinaria}-{i * 1000}",
                            EspecificacionesTecnicas = $"Especificaciones técnicas del componente {compCounter}",
                            FechaUltimoCambio = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(-i * 15), DateTimeKind.Utc),
                            IdEstado = estados[i % estados.Count].IdEstado,
                            IdProveedor = proveedor?.IdProveedor,
                            Activo = true
                        });
                    }
                    compCounter++;
                }
            }
            context.SaveChanges();
        }
    }
}
