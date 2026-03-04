namespace Orión.Domain.Entities;

public class Ubicacion
{
    public int IdUbicacion { get; set; }
    public int NumeroNave { get; set; }

    // Relaciones
    public virtual ICollection<Maquinaria> Maquinarias { get; set; } = new List<Maquinaria>();
}
