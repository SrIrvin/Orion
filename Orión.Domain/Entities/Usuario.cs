namespace Orión.Domain.Entities;

public class Usuario : BaseEntity
{
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Rol { get; set; } = "Operador"; // Roles: Admin, Tecnico, Operador

    // Seguridad Proactiva
    public DateTime? LastPasswordChange { get; set; }
    public bool RequiresPasswordChange { get; set; } = false;

    // Auditoría básica
    public bool Activo { get; set; } = true;
}
