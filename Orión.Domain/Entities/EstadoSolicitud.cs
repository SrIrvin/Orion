namespace Orión.Domain.Entities;

public class EstadoSolicitud
{
    public int IdEstadoSolicitud { get; set; }
    public string DescripcionEstado { get; set; } = string.Empty;

    // Relaciones
    public virtual ICollection<SolicitudServicio> SolicitudesServicio { get; set; } = new List<SolicitudServicio>();
}
