using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorneosFutbol.Domain.Entities
{
    public class Fecha
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; } // Ej: "Fecha 1", "Cuartos de final"
        public int Orden { get; private set; }     // Para saber cuál va primero (1, 2, 3...)

        public Fecha(string nombre, int orden)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre de la fecha es obligatorio.");

            Id = Guid.NewGuid();
            Nombre = nombre;
            Orden = orden;
        }

        private Fecha() { } // Requerido para el ORM
    }
}
