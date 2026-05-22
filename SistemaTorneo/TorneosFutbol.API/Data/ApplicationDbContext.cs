using Microsoft.EntityFrameworkCore;
using ProdeMundial.Data; // Asegurate de que coincida con tu namespace
using TorneosFutbol.API.Models;

namespace ProdeMundial.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // 📝 Estos DbSet se transforman en tus tablas de Postgres
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Partido> Partidos { get; set; }
        public DbSet<Prediccion> Predicciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuramos una clave compuesta o restricciones si hiciera falta.
            // Por ejemplo, que el nombre del usuario sea único para que no se registren repetidos:
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Nombre)
                .IsUnique();
        }
    }
}