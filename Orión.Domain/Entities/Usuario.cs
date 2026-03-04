namespace Orión.Domain.Entities;

public class Usuario
{
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Rol { get; set; } = "Operador"; // Roles: Admin, Tecnico, Operador

    // Auditoría básica
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public bool Activo { get; set; } = true;
}
