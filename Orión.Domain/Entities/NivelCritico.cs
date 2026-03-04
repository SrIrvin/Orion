namespace Orión.Domain.Entities;

public class NivelCritico
{
    public int IdNivelCritico { get; set; }
    public string Descripcion { get; set; } = string.Empty;

    // Relaciones
    public virtual ICollection<Maquinaria> Maquinarias { get; set; } = new List<Maquinaria>();
}
