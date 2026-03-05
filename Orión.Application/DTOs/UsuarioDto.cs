namespace Orión.Application.DTOs;

public class UsuarioDto
{
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Rol { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public DateTime CreatedAt { get; set; }
}
