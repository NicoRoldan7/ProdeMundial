using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorneosFutbol.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; }

        public Usuario(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre de usuario no puede estar vacío.");

            Id = Guid.NewGuid();
            Nombre = nombre;
        }

        private Usuario() { } // Requerido para el ORM
    }
}
