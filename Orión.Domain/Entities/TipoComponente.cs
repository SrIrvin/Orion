namespace Orión.Domain.Entities;

public class TipoComponente
{
    public int IdTipoComponente { get; set; }
    public string NombreTipo { get; set; } = string.Empty;

    // Relaciones
    public virtual ICollection<Componente> Componentes { get; set; } = new List<Componente>();
}
