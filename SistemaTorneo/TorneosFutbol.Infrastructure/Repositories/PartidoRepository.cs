using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TorneosFutbol.Domain.Entities;
using TorneosFutbol.Domain.Ports.Out;
using TorneosFutbol.Infrastructure.Data;

namespace TorneosFutbol.Infrastructure.Repositories
{
    public class PartidoRepository : IPartidoRepository
    {
        private readonly AppDbContext _context;
        public PartidoRepository(AppDbContext context) => _context = context;

        public async Task GuardarAsync(Partido partido)
        {
            await _context.Partidos.AddAsync(partido);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Partido>> ObtenerTodosAsync() => await _context.Partidos.ToListAsync();
        public async Task<Partido?> ObtenerPorIdAsync(Guid id) => await _context.Partidos.FindAsync(id);

        public async Task ActualizarAsync(Partido partido)
        {
            _context.Partidos.Update(partido);
            await _context.SaveChangesAsync();
        }
    }
}