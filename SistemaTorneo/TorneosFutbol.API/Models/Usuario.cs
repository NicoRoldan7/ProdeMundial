namespace TorneosFutbol.API.Models;
public class Usuario
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nombre { get; set; } = string.Empty;
    public int PuntajeTotal { get; set; } = 0; // Se actualiza sumando las predicciones
}
