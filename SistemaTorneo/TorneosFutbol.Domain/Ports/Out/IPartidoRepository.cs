using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
