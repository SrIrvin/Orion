namespace Orión.Domain.Entities;

public class Maquinaria
{
    public string IdMaquinaria { get; set; } = string.Empty;
    public string NombreMaquina { get; set; } = string.Empty;
    public string? Tipo { get; set; }
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public DateTime? FechaInstalacion { get; set; }
    public bool Activo { get; set; } = true;
    
    // Foreign Keys
    public int IdNivelCritico { get; set; }
    public int IdUbicacion { get; set; }

    // Propiedades de Navegación
    public virtual NivelCritico NivelCritico { get; set; } = null!;
    public virtual Ubicacion Ubicacion { get; set; } = null!;
    
    public virtual ICollection<Componente> Componentes { get; set; } = new List<Componente>();
    public virtual ICollection<SolicitudServicio> SolicitudesServicio { get; set; } = new List<SolicitudServicio>();
}
