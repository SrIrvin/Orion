namespace Orión.Domain.Entities;

public class Proveedor : BaseEntity
{
    public int IdProveedor { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? RUC { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
    public bool Activo { get; set; } = true;

    // Propiedades de Navegación
    public virtual ICollection<Componente> Componentes { get; set; } = new List<Componente>();
}
