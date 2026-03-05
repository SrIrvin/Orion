namespace Orión.Application.DTOs;

public class ProveedorDto
{
    public int IdProveedor { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? RUC { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
    public bool Activo { get; set; }
}
