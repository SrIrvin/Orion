namespace Orión.Domain.Entities;

public class EstadoComponente
{
    public int IdEstado { get; set; }
    public string DescripcionEstado { get; set; } = string.Empty;

    // Relaciones
    public virtual ICollection<Componente> Componentes { get; set; } = new List<Componente>();
}
