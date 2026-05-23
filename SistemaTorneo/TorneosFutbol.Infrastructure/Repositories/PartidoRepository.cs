using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TorneosFutbol.Domain.Entities;
using TorneosFutbol.Domain.Ports.Out;
using TorneosFutbol.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using TorneosFutbol.Domain.Entities;
using TorneosFutbol.Domain.Ports.Out; // Tu carpeta de puertos de salida

namespace TorneosFutbol.Infrastructure.Repositories
{
    public class PartidoRepository : IPartidoRepository
    {
        private readonly AppDbContext _context;

        public PartidoRepository(AppDbContext context)
        {
            _context = context;
        }

        // 1. Obtener todos los partidos (El que va a usar el controlador para el Front)
        public async Task<IEnumerable<Partido>> ObtenerTodosAsync()
        {
            return await _context.Partidos.ToListAsync();
        }

        // 2. Obtener un partido puntual por su ID
        public async Task<Partido?> ObtenerPorIdAsync(Guid id)
        {
            return await _context.Partidos.FirstOrDefaultAsync(p => p.Id == id);
        }

        // 3. Guardar un partido nuevo
        public async Task GuardarAsync(Partido partido)
        {
            await _context.Partidos.AddAsync(partido);
            await _context.SaveChangesAsync();
        }

        // 4. Actualizar el partido (cuando cargues los goles reales al final)
        public async Task ActualizarAsync(Partido partido)
        {
            _context.Partidos.Update(partido);
            await _context.SaveChangesAsync();
        }
    }
}