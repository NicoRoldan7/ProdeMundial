using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TorneosFutbol.Domain.Entities;


namespace TorneosFutbol.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Registramos todas las tablas del Prode
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Fecha> Fechas { get; set; }
        public DbSet<Partido> Partidos { get; set; }
        public DbSet<Prediccion> Predicciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Configuración de Equipos (Ya la teníamos)
            modelBuilder.Entity<Equipo>(entity =>
            {
                entity.ToTable("Equipos");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LogoUrl).HasMaxLength(500);
            });

            // 2. Configuración de Usuarios
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Nombre).IsRequired().HasMaxLength(50);
            });

            // 3. Configuración de Fechas
            modelBuilder.Entity<Fecha>(entity =>
            {
                entity.ToTable("Fechas");
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Nombre).IsRequired().HasMaxLength(100);
            });

            // 4. Configuración de Partidos
            modelBuilder.Entity<Partido>(entity =>
            {
                entity.ToTable("Partidos");
                entity.HasKey(p => p.Id);
                // No agregamos relaciones complejas de FK acá para mantener la base en memoria 
                // ultra liviana y simple de manipular desde el código.
            });

            // 5. Configuración de Predicciones
            modelBuilder.Entity<Prediccion>(entity =>
            {
                entity.ToTable("Predicciones");
                entity.HasKey(pr => pr.Id);
            });
        }
    }
}