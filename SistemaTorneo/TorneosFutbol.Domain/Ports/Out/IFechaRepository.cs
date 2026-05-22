using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TorneosFutbol.Domain.Entities;

namespace TorneosFutbol.Domain.Ports.Out
{
    public interface IFechaRepository
    {
        Task GuardarAsync(Fecha fecha);
        Task<IEnumerable<Fecha>> ObtenerTodasAsync();
    }
}
