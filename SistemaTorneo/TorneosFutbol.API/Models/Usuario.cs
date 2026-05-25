namespace TorneosFutbol.API.Models;
public class Usuario
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nombre { get; set; } = string.Empty;
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime FechaRegistro { get; set; }
    public int PuntajeTotal { get; set; } = 0; // Se actualiza sumando las predicciones
}
