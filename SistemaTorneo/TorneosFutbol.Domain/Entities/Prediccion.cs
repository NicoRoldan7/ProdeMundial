using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorneosFutbol.Domain.Entities
{
    public class Prediccion
    {
        public Guid Id { get; private set; }
        public Guid UsuarioId { get; private set; }
        public Guid PartidoId { get; private set; }
        public int GolesLocalPrediccion { get; private set; }
        public int GolesVisitantePrediccion { get; private set; }
        public int PuntosGanados { get; private set; }
        public Guid? TorneoId { get; set; }

        public Prediccion(Guid usuarioId, Guid partidoId, int golesLocal, int golesVisitante)
        {
            Id = Guid.NewGuid();
            UsuarioId = usuarioId;
            PartidoId = partidoId;
            GolesLocalPrediccion = golesLocal;
            GolesVisitantePrediccion = golesVisitante;
            PuntosGanados = 0; // Arranca en cero hasta que juegue el partido real
        }

        // LA REGLA DE ORO DEL PRODE: Se ejecuta cuando el partido real finaliza
        public void CalcularPuntos(Partido partido)
        {
            if (partido == null || !partido.Finalizado)
            {
                PuntosGanados = 0;
                return;
            }

            // Capturamos los resultados reales seguros (sabemos que no son null porque Finalizado == true)
            int realLocal = partido.GolesLocalReal!.Value;
            int realVisitante = partido.GolesVisitanteReal!.Value;

            // CASO 1: ¡Pleno! Pegaron el resultado exacto (Ej: Predicción 2-1 y Real 2-1)
            if (GolesLocalPrediccion == realLocal && GolesVisitantePrediccion == realVisitante)
            {
                PuntosGanados = 3;
                return;
            }

            // Calculamos la tendencia real (1 = Gana Local, -1 = Gana Visitante, 0 = Empate)
            int tendenciaReal = realLocal.CompareTo(realVisitante);
            // Calculamos la tendencia que predijo el usuario
            int tendenciaPrediccion = GolesLocalPrediccion.CompareTo(GolesVisitantePrediccion);

            // CASO 2: Acertó quién ganaba o si empataban, pero erró la cantidad de goles
            if (tendenciaReal == tendenciaPrediccion)
            {
                PuntosGanados = 1;
            }
            // CASO 3: No pegó nada
            else
            {
                PuntosGanados = 0;
            }
        }

        private Prediccion() { } // Requerido para el ORM
    }
}
