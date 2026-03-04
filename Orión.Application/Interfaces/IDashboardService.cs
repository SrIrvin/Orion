namespace Orión.Application.Interfaces;

public interface IDashboardService
{
    Task<int> GetTotalMaquinariaAsync();
    Task<int> GetTotalTecnicosAsync();
    Task<int> GetSolicitudesAbiertasAsync();
}
