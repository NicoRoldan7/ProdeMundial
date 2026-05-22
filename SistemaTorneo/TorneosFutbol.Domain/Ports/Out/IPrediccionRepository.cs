using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TorneosFutbol.Domain.Entities;

namespace TorneosFutbol.Domain.Ports.Out
{
    public interface IPrediccionRepository
    {
        Task GuardarAsync(Prediccion prediccion);
        Task<IEnumerable<Prediccion>> ObtenerPorUsuarioAsync(Guid usuarioId);
        Task<IEnumerable<Prediccion>> ObtenerPorPartidoAsync(Guid partidoId);
        Task ActualizarAsync(Prediccion prediccion); // Para actualizarle los puntos cuando el partido termine
    }
}
