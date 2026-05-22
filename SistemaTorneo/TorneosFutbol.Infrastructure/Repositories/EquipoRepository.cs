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
    // Usamos la palabra "implements" implícitamente al poner los dos puntos (:). 
    // Esta clase promete cumplir con el contrato del Dominio.
    public class EquipoRepository : IEquipoRepository
    {
        private readonly AppDbContext _context;

        // Inyectamos el contexto de la base de datos que creamos recién
        public EquipoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task GuardarAsync(Equipo equipo)
        {
            await _context.Equipos.AddAsync(equipo);
            await _context.SaveChangesAsync(); // Guarda los cambios reales en SQL Server
        }

        public async Task<IEnumerable<Equipo>> ObtenerTodosAsync()
        {
            return await _context.Equipos.ToListAsync();
        }

        public async Task<Equipo?> ObtenerPorIdAsync(Guid id)
        {
            return await _context.Equipos.FindAsync(id);
        }
    }
}
