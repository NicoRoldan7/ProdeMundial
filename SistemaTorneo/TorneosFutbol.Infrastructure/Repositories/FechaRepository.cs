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
    public class FechaRepository : IFechaRepository
    {
        private readonly AppDbContext _context;
        public FechaRepository(AppDbContext context) => _context = context;

        public async Task GuardarAsync(Fecha fecha)
        {
            await _context.Fechas.AddAsync(fecha);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Fecha>> ObtenerTodasAsync() =>
            await _context.Fechas.OrderBy(f => f.Orden).ToListAsync(); // Los trae ordenados por fecha 1, 2, 3...
    }
}
