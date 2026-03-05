using Microsoft.EntityFrameworkCore;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;

namespace Orión.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IOrionDbContext _context;

    public DashboardService(IOrionDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetTotalMaquinariaAsync()
    {
        return await _context.Maquinarias.CountAsync();
    }

    public async Task<int> GetTotalTecnicosAsync()
    {
        return await _context.Tecnicos.CountAsync();
    }

    public async Task<int> GetSolicitudesAbiertasAsync()
    {
        return await _context.SolicitudesServicios.CountAsync(s => s.IdEstadoSolicitud == 1);
    }

    public async Task<IEnumerable<MachineHealthHeatmapDto>> GetMachineHealthHeatmapAsync()
    {
        var maquinas = await _context.Maquinarias.Where(m => m.Activo).ToListAsync();
        
        // Calcular inicio de la semana actual (Domingo) en UTC
        var now = DateTime.UtcNow;
        var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
        
        int diff = (7 + (today.DayOfWeek - DayOfWeek.Sunday)) % 7;
        var startOfWeek = today.AddDays(-1 * diff);
        var endOfWeek = startOfWeek.AddDays(7);

        var solicitudesSemana = await _context.SolicitudesServicios
            .Where(s => s.FechaApertura >= startOfWeek && s.FechaApertura < endOfWeek)
            .ToListAsync();

        var heatmap = new List<MachineHealthHeatmapDto>();

        foreach (var maq in maquinas)
        {
            var item = new MachineHealthHeatmapDto
            {
                IdMaquinaria = maq.IdMaquinaria,
                NombreMaquinaria = maq.NombreMaquina,
                Days = new List<DayHealthDto>()
            };

            for (int i = 0; i < 7; i++)
            {
                var day = (DayOfWeek)i;
                var currentDayDate = startOfWeek.AddDays(i);
                
                var count = solicitudesSemana.Count(s => 
                    s.IdMaquinaria == maq.IdMaquinaria && 
                    s.FechaApertura.Date == currentDayDate);

                item.Days.Add(new DayHealthDto
                {
                    Day = day,
                    FailureCount = count,
                    ColorLevel = GetColorForFailures(count)
                });
            }
            heatmap.Add(item);
        }

        return heatmap;
    }

    public async Task<IEnumerable<GlobalActivityHeatmapDto>> GetGlobalActivityHeatmapAsync()
    {
        // Calcular las últimas 30 semanas completas
        var now = DateTime.UtcNow;
        var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
        
        int diffToSunday = (7 + (today.DayOfWeek - DayOfWeek.Sunday)) % 7;
        var startOfCurrentWeek = today.AddDays(-1 * diffToSunday);
        var startDate = startOfCurrentWeek.AddDays(-7 * 29); // 30 semanas total
        
        var solicitudes = await _context.SolicitudesServicios
            .Where(s => s.FechaApertura >= startDate)
            .ToListAsync();

        var columns = new List<GlobalActivityHeatmapDto>();
        string[] dayNames = { "Dom", "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb" };

        for (int w = 0; w < 30; w++) // 30 Columnas (Semanas)
        {
            var weekStartDate = startDate.AddDays(w * 7);
            var column = new GlobalActivityHeatmapDto
            {
                DateHeader = weekStartDate.ToString("dd/MM"),
                Days = new List<CalendarDayDto>()
            };

            for (int d = 0; d < 7; d++) // 7 Filas por columna
            {
                var currentDay = weekStartDate.AddDays(d);
                var count = solicitudes.Count(s => s.FechaApertura.Date == currentDay.Date);

                column.Days.Add(new CalendarDayDto
                {
                    Date = currentDay,
                    DayName = dayNames[d],
                    FailureCount = count,
                    ColorLevel = GetColorForFailures(count)
                });
            }
            columns.Add(column);
        }

        return columns;
    }

    private string GetColorForFailures(int count)
    {
        return count switch
        {
            0 => "#EBEDF0", // Sin fallas (Gris)
            1 => "#FFF4D1", // 1 falla (Amarillo muy claro)
            2 => "#FFD082", // 2 fallas (Naranja claro)
            3 => "#FF8A65", // 3 fallas (Naranja fuerte)
            _ => "#E53935"  // 4+ fallas (Rojo crítico)
        };
    }
}
