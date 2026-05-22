namespace TorneosFutbol.API.Models;
public class Equipo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nombre { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
}
