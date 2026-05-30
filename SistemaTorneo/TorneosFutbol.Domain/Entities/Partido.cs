using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorneosFutbol.Domain.Entities
{
    public class Partido
    {
        public Guid Id { get; private set; }
        public Guid FechaId { get; private set; }
        public Guid LocalId { get; private set; }
        public Guid VisitanteId { get; private set; }

        public int? GolesLocalReal { get; private set; }
        public int? GolesVisitanteReal { get; private set; }
        public bool Finalizado { get; private set; }
        public string Fecha { get; private set; }
        public string Hora { get; private set; }

        public Partido(Guid fechaId, Guid localId, Guid visitanteId, string Fecha, string Hora)
        {
            if (localId == Guid.Empty || visitanteId == Guid.Empty || fechaId == Guid.Empty)
                throw new ArgumentException("Los IDs de la fecha y de las selecciones son obligatorios.");

            if (localId == visitanteId)
                throw new ArgumentException("Una selección no puede jugar contra sí misma.");

            Id = Guid.NewGuid();
            FechaId = fechaId;
            LocalId = localId;
            VisitanteId = visitanteId;
            Finalizado = false;
            Fecha = Fecha;
            Hora = Hora;
        }

        // Método para cuando termine el partido real. Vos cargás el resultado acá.
        public void RegistrarResultado(int golesLocal, int golesVisitante)
        {
            GolesLocalReal = golesLocal;
            GolesVisitanteReal = golesVisitante;
            Finalizado = true;
        }

        private Partido() { } // Requerido para el ORM
    }
}
