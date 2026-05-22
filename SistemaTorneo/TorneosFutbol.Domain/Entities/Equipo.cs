using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorneosFutbol.Domain.Entities
{
    public class Equipo
    {
        // Usamos Guid (un identificador único global) en lugar de un simple número 1, 2, 3. 
        // Esto es un estándar moderno en sistemas de alta escala.
        public Guid Id { get; private set; }
        public string Nombre { get; private set; }
        public string LogoUrl { get; private set; }

        // Este es el constructor. Obligamos a que cualquier persona que cree un equipo
        // nos pase sí o sí un nombre y una foto.
        public Equipo(string nombre, string logoUrl)
        {
            // Regla de negocio básica: Un equipo no puede no tener nombre.
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del equipo es obligatorio.");
            }

            Id = Guid.NewGuid(); // Generamos el ID único automáticamente al crearlo
            Nombre = nombre;
            LogoUrl = string.IsNullOrWhiteSpace(logoUrl) ? "url_por_defecto.png" : logoUrl;
        }

        // Este constructor vacío y privado lo dejamos acá porque más adelante
        // Entity Framework (la base de datos) lo va a necesitar para reconstruir el objeto.
        private Equipo() { }
    }
}
