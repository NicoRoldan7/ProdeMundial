namespace TorneosFutbol.API.Models;
public class Partido
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Relaciones con Equipos
    public Guid LocalId { get; set; }
    public Guid VisitanteId { get; set; }

    // Goles REALES del partido (pueden ser null hasta que se juegue)
    public int? GolesLocalReal { get; set; }
    public int? GolesVisitanteReal { get; set; }

    public DateTime FechaPartido { get; set; }
}
