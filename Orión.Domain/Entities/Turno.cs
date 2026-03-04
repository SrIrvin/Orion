namespace Orión.Domain.Entities;

public class Turno
{
    public int IdTurno { get; set; }
    public string DescripcionTurno { get; set; } = string.Empty;

    // Relaciones
    public virtual ICollection<Tecnico> Tecnicos { get; set; } = new List<Tecnico>();
}
