using TorneosFutbol.Domain.Entities;

namespace TorneosFutbol.Domain.Ports.Out
{
    public interface IPartidoRepository
    {
        Task GuardarAsync(Partido partido);
        Task<IEnumerable<Partido>> ObtenerTodosAsync();
        Task<Partido?> ObtenerPorIdAsync(Guid id);
        Task ActualizarAsync(Partido partido); // Necesario para guardar el resultado cuando termine
    }
}
