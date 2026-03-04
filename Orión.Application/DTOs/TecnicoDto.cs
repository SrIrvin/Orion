namespace Orión.Application.DTOs;

public class TecnicoDto
{
    public int IdPersonal { get; set; }
    public string NombreApellido { get; set; } = string.Empty;
    public string? Especialidad { get; set; }
    public string TurnoDescripcion { get; set; } = string.Empty;
}
