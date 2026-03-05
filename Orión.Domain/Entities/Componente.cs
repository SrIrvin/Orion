namespace Orión.Domain.Entities;

public class Componente : BaseEntity
{
    public string IdComponente { get; set; } = string.Empty;
    public string NombreComponente { get; set; } = string.Empty;
    public string? Marca { get; set; }
    public string? NumeroSerie { get; set; }
    public string? EspecificacionesTecnicas { get; set; }
    public DateTime? FechaUltimoCambio { get; set; }
    public bool Activo { get; set; } = true;

    // Foreign Keys
    public string IdMaquinaria { get; set; } = string.Empty;
    public int IdTipoComponente { get; set; }
    public int IdEstado { get; set; }
    public int? IdProveedor { get; set; }

    // Propiedades de Navegación
    public virtual Maquinaria Maquinaria { get; set; } = null!;
    public virtual TipoComponente TipoComponente { get; set; } = null!;
    public virtual EstadoComponente EstadoComponente { get; set; } = null!;
    public virtual Proveedor? Proveedor { get; set; }
}
