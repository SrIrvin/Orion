namespace Orión.Application.DTOs;

public class ComponenteDto
{
    public string IdComponente { get; set; } = string.Empty;
    public string NombreComponente { get; set; } = string.Empty;
    public string? Marca { get; set; }
    public string? NumeroSerie { get; set; }
    public string? EspecificacionesTecnicas { get; set; }
    public DateTime? FechaUltimoCambio { get; set; }
    public string IdMaquinaria { get; set; } = string.Empty;
    public string TipoComponenteNombre { get; set; } = string.Empty;
    public string EstadoDescripcion { get; set; } = string.Empty;
}
