using Orión.Application.DTOs;

namespace Orión.Application.Interfaces;

public interface IReportService
{
    void GenerateSolicitudPdf(SolicitudServicioDto solicitud, string filePath);
}
