using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorneosFutbol.Domain.Entities
{
    public class TorneoParticipante
    {
        [Column("torneo_id")]
        public Guid TorneoId { get; set; }
        [Column("usuario_id")]
        public Guid UsuarioId { get; set; } // Ajustado para seguir tu convención de nombres
        public string Rol { get; set; } = "participante";

        // Constructor para inicializar
        public TorneoParticipante(Guid torneoId, Guid usuarioId)
        {
            TorneoId = torneoId;
            UsuarioId = usuarioId;
        }

        public TorneoParticipante() { }
    }
}