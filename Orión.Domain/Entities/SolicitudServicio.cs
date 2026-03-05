namespace Orión.Domain.Entities;

public class SolicitudServicio : BaseEntity
{
    public int IdSS { get; set; }
    public string DescripcionFalla { get; set; } = string.Empty;
    public DateTime FechaApertura { get; set; }
    public DateTime? FechaCierre { get; set; }

    // Foreign Keys
    public string IdMaquinaria { get; set; } = string.Empty;
    public int IdTipoMantto { get; set; }
    public int? IdPersonal { get; set; } // Opcional si no se asigna de inmediato
    public int IdEstadoSolicitud { get; set; }

    // Propiedades de Navegación
    public virtual Maquinaria Maquinaria { get; set; } = null!;
    public virtual TipoMantenimiento TipoMantenimiento { get; set; } = null!;
    public virtual Tecnico? Tecnico { get; set; }
    public virtual EstadoSolicitud EstadoSolicitud { get; set; } = null!;
}
