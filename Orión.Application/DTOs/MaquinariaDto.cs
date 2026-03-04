namespace Orión.Application.DTOs;

public class MaquinariaDto
{
    public string IdMaquinaria { get; set; } = string.Empty;
    public string NombreMaquina { get; set; } = string.Empty;
    public string? Tipo { get; set; }
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public DateTime? FechaInstalacion { get; set; }
    public int IdNivelCritico { get; set; }
    public string NivelCriticoDescripcion { get; set; } = string.Empty;
    public int IdUbicacion { get; set; }
    public string UbicacionNave { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
}
