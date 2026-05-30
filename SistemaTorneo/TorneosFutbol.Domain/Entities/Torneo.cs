using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorneosFutbol.Domain.Entities
{
    public class Torneo
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("nombre_torneo")]
        public string NombreTorneo { get; set; }

        [Required]
        public Guid CreadorId { get; set; }

        // Este es el token que vamos a usar para los links de invitación
        [Column("token_acceso")]
        public string TokenAcceso { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public Torneo()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        // Constructor para cuando lo creás manualmente
        public Torneo(string nombre, Guid creadorId)
        {
            Id = Guid.NewGuid();
            NombreTorneo = nombre;
            CreadorId = creadorId;
            CreatedAt = DateTime.UtcNow;
            TokenAcceso = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }
    }
}