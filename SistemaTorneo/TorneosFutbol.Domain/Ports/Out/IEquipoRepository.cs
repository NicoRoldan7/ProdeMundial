using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TorneosFutbol.Domain.Entities;

namespace TorneosFutbol.Domain.Ports.Out
{
    public interface IEquipoRepository
    {
        // Definimos los métodos asíncronos (Task), fundamentales hoy en día 
        // para que la aplicación sea ultra rápida y no se bloquee bajo mucha carga.
        Task GuardarAsync(Equipo equipo);
        Task<IEnumerable<Equipo>> ObtenerTodosAsync();
        Task<Equipo?> ObtenerPorIdAsync(Guid id);
    }
}