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

        // 3. Generar Solicitudes de Servicio (Historial masivo)
        if (!context.SolicitudesServicios.Any())
        {
            var maquinasParaSolicitudes = todasLasMaquinas.Take(10).ToList();
            var tecnicos = context.Tecnicos.ToList();

            for (int i = 1; i <= 30; i++)
            {
                context.SolicitudesServicios.Add(new SolicitudServicio
                {
                    IdMaquinaria = maquinasParaSolicitudes[i % maquinasParaSolicitudes.Count].IdMaquinaria,
                    IdTipoMantto = 1,
                    DescripcionFalla = $"Falla técnica detectada en ciclo de prueba STG-{i}",
                    FechaApertura = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(-i), DateTimeKind.Utc),
                    IdPersonal = tecnicos[i % tecnicos.Count].IdPersonal,
                    IdEstadoSolicitud = 1
                });
            }
            context.SaveChanges();
        }

        // 4. Generar Componentes (Asegurar que TODAS las máquinas tengan al menos uno)
        var tipos = context.TiposComponentes.ToList();
        var estados = context.EstadosComponentes.ToList();

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
