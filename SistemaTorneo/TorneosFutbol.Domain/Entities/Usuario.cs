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
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // 🔥 MODIFICAMOS EL CONSTRUCTOR PARA RECIBIR TODO:
        public Usuario(string nombre, string username, string email, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("El nombre de usuario (username) no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El correo electrónico no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("La contraseña no puede estar vacía.");

            Id = Guid.NewGuid();
            Nombre = nombre;
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            FechaRegistro = DateTime.UtcNow; // Nos aseguramos de guardar la fecha/hora exacta de creación
        }

        private Usuario() { } // Requerido para el ORM (Entity Framework)
    }
}
