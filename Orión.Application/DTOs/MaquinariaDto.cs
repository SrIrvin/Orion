namespace Orión.Application.DTOs;

public class MaquinariaDto
{
    public string IdMaquinaria { get; set; } = string.Empty;
    public string NombreMaquina { get; set; } = string.Empty;
    public string? Tipo { get; set; }
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public DateTime? FechaInstalacion { get; set; }
    public string NivelCriticoDescripcion { get; set; } = string.Empty;
    public string UbicacionNave { get; set; } = string.Empty;
}
