namespace TorneosFutbol.API.Models;
public class Equipo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nombre { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;

    public string Grupo { get; set; } = string.Empty;

    public Equipo(string nombre, string logoUrl, string grupo)
    {
        Nombre = nombre;
        LogoUrl = logoUrl;
        Grupo = grupo; // Asignación necesaria
    }
}
