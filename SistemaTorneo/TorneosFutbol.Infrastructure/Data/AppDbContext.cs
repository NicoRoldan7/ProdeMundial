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

            // 1. Configuración de Equipos (CORREGIDO EN MINÚSCULAS)
            modelBuilder.Entity<Equipo>(entity =>
            {
                entity.ToTable("equipos"); // En minúscula 'equipos'
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id"); // <--- CLAVE: id en minúscula
                entity.Property(e => e.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(100);
                entity.Property(e => e.LogoUrl).HasColumnName("logo_url").HasMaxLength(500); // <--- Ojo acá, va con guion bajo
            });

            // 2. Configuración de Usuarios (CORREGIDO EN MINÚSCULAS)
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios"); // En minúscula 'usuarios'
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).HasColumnName("id"); // <--- CLAVE: id en minúscula
                entity.Property(u => u.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(50);
            });

            // 3. Configuración de Fechas (CORREGIDO EN MINÚSCULAS)
            modelBuilder.Entity<Fecha>(entity =>
            {
                entity.ToTable("fechas"); // En minúscula 'fechas'
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Id).HasColumnName("id"); // <--- CLAVE: id en minúscula
                entity.Property(f => f.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(100);
                // Si tu entidad Fecha tiene la propiedad Orden, agregala acá abajo:
                // entity.Property(f => f.Orden).HasColumnName("orden");
            });

            // 4. Configuración de Partidos
            modelBuilder.Entity<Partido>(entity =>
            {
                // Forzamos el nombre de la tabla idéntico al de Supabase (en minúsculas)
                entity.ToTable("partidos");

                entity.HasKey(p => p.Id);

                // Mapeamos cada propiedad de C# con su columna real en Postgres
                entity.Property(p => p.Id).HasColumnName("id");
                entity.Property(p => p.FechaId).HasColumnName("fecha_id").IsRequired();
                entity.Property(p => p.LocalId).HasColumnName("local_id").IsRequired();
                entity.Property(p => p.VisitanteId).HasColumnName("visitante_id").IsRequired();

                entity.Property(p => p.GolesLocalReal).HasColumnName("goles_local_real");
                entity.Property(p => p.GolesVisitanteReal).HasColumnName("goles_visitante_real");
                entity.Property(p => p.Finalizado).HasColumnName("finalizado");
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