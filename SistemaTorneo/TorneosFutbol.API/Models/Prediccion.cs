using System;

public class Prediccion
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Quién y en qué partido jugó
    public Guid UsuarioId { get; set; }
    public Guid PartidoId { get; set; }

    // Lo que arriesgó el usuario
    public int GolesLocalVoto { get; set; }
    public int GolesVisitanteVoto { get; set; }

    // Puntos obtenidos en este partido (Ej: 3 por resultado exacto, 1 por ganador, 0 si erró)
    public int PuntosGanados { get; set; } = 0;
}
