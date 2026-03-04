namespace Orión.Domain.Entities;

public class TipoMantenimiento
{
    public int IdTipoMantto { get; set; }
    public string DescripcionTipo { get; set; } = string.Empty;

    // Relaciones
    public virtual ICollection<SolicitudServicio> SolicitudesServicio { get; set; } = new List<SolicitudServicio>();
}
