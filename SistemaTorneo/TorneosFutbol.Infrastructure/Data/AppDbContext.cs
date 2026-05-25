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

            // 1. Configuración de Usuarios (Mapeado EXACTO a tu captura: "Id" y "Nombre")
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios"); // Nombre de la tabla con U mayúscula
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).HasColumnName("Id"); // La I es MAYÚSCULA en Supabase
                entity.Property(u => u.Nombre).HasColumnName("Nombre").IsRequired().HasMaxLength(50); // La N es MAYÚSCULA
            });

            // 2. Configuración de Equipos (Mapeado EXACTO a tu captura: "id", "nombre", "logo_url")
            modelBuilder.Entity<Equipo>(entity =>
            {
                entity.ToTable("Equipos"); // Nombre de la tabla con E mayúscula
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id"); // Acá va en minúscula
                entity.Property(e => e.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(100); // Minúscula
                entity.Property(e => e.LogoUrl).HasColumnName("logo_url").HasMaxLength(500); // Minúscula
            });

            // 3. Configuración de Fechas (Mapeado EXACTO a tu captura: "id", "nombre")
            modelBuilder.Entity<Fecha>(entity =>
            {
                entity.ToTable("Fechas"); // Nombre de la tabla con F mayúscula
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Id).HasColumnName("id"); // Minúscula
                entity.Property(f => f.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(100); // Minúscula
            });

            // 4. Configuración de Partidos 
            // 4. Configuración de Partidos (CALIBRADO AL 100% CON TUS MINÚSCULAS)
            modelBuilder.Entity<Partido>(entity =>
            {
                entity.ToTable("partidos"); // <--- ¡AQUÍ! Cambiado a "partidos" con la p MINÚSCULA
                entity.HasKey(p => p.Id);

                // Todo el resto de las columnas en minúsculas como tu captura:
                entity.Property(p => p.Id).HasColumnName("id");
                entity.Property(p => p.FechaId).HasColumnName("fecha_id");
                entity.Property(p => p.LocalId).HasColumnName("local_id");
                entity.Property(p => p.VisitanteId).HasColumnName("visitante_id");
                entity.Property(p => p.GolesLocalReal).HasColumnName("goles_local_real");
                entity.Property(p => p.GolesVisitanteReal).HasColumnName("goles_visitante_real");
                entity.Property(p => p.Finalizado).HasColumnName("finalizado");
            });

            // 5. Configuración de Predicciones (Mapeado EXACTO a tu captura de Predicciones)
            modelBuilder.Entity<Prediccion>(entity =>
            {
                entity.ToTable("Predicciones"); // Nombre de la tabla con P mayúscula
                entity.HasKey(pr => pr.Id);
                entity.Property(pr => pr.Id).HasColumnName("Id"); // I Mayúscula
                entity.Property(pr => pr.UsuarioId).HasColumnName("UsuarioId"); // Combinado mayúsculas
                entity.Property(pr => pr.PartidoId).HasColumnName("PartidoId"); // Combinado mayúsculas
                entity.Property(pr => pr.GolesLocalPrediccion).HasColumnName("GolesLocalPrediccion");
                entity.Property(pr => pr.GolesVisitantePrediccion).HasColumnName("GolesVisitantePrediccion");
                entity.Property(pr => pr.PuntosGanados).HasColumnName("PuntosGanados");
            });
        }
    }

}