using Orión.Application.DTOs;

namespace Orión.Application.Interfaces;

public interface IDashboardService
{
    Task<int> GetTotalMaquinariaAsync();
    Task<int> GetTotalTecnicosAsync();
    Task<int> GetSolicitudesAbiertasAsync();
    Task<IEnumerable<MachineHealthHeatmapDto>> GetMachineHealthHeatmapAsync();
    Task<IEnumerable<GlobalActivityHeatmapDto>> GetGlobalActivityHeatmapAsync();
}
