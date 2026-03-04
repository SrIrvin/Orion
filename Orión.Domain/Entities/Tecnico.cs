namespace Orión.Domain.Entities;

public class Tecnico
{
    public int IdPersonal { get; set; }
    public string NombreApellido { get; set; } = string.Empty;
    public string? Especialidad { get; set; }
    
    // Foreign Key
    public int IdTurno { get; set; }

    public bool Activo { get; set; } = true;

    // Propiedades de Navegación
    public virtual Turno Turno { get; set; } = null!;
    public virtual ICollection<SolicitudServicio> SolicitudesServicio { get; set; } = new List<SolicitudServicio>();
}
