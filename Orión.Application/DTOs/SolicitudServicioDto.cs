namespace Orión.Application.DTOs;

public class SolicitudServicioDto
{
    public int IdSS { get; set; }
    public string DescripcionFalla { get; set; } = string.Empty;
    public DateTime FechaApertura { get; set; }
    public DateTime? FechaCierre { get; set; }
    
    public string IdMaquinaria { get; set; } = string.Empty;
    public string NombreMaquinaria { get; set; } = string.Empty;
    
    public int IdTipoMantto { get; set; }
    public string TipoMantenimientoDescripcion { get; set; } = string.Empty;
    
    public int? IdPersonal { get; set; }
    public string TecnicoNombre { get; set; } = "No asignado";
    
    public int IdEstadoSolicitud { get; set; }
    public string EstadoDescripcion { get; set; } = string.Empty;
}
