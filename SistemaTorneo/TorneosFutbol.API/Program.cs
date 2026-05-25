using Microsoft.EntityFrameworkCore;
using TorneosFutbol.Domain.Entities;
using TorneosFutbol.Domain.Ports.Out;
using TorneosFutbol.Infrastructure.Data;
using TorneosFutbol.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// 1. REGISTRAR SERVICIOS (Contenedor de Dependencias)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "🏆 PRODE MUNDIAL 2026",
        Version = "v1",
        Description = "Backend oficial. Gestión de usuarios, fixture y cálculo automático de puntos."
    });
});

// 🔌 CONEXIÓN PROFESIONAL A POSTGRESQL (En reemplazo de la base en memoria)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

// ENCHUFES HEXAGONALES (Mapeo de Interfaces con Repositorios Reales)
builder.Services.AddScoped<IEquipoRepository, EquipoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IFechaRepository, FechaRepository>();
builder.Services.AddScoped<IPartidoRepository, PartidoRepository>();

// Seguridad CORS abierta para conectar tu Frontend sin trabas
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

var predicciones = new List<TorneosFutbol.Domain.Entities.Prediccion>();

// 2. CONFIGURAR EL PIPELINE DE HTTP
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
// Habilitar Swagger siempre (tanto en desarrollo como en producción en Render)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Torneos Futbol API v1");
    c.RoutePrefix = string.Empty; // <-- ESTO ES UN TRUCAZO: Hace que Swagger abra directo en la URL principal sin poner /swagger
});

app.UseCors();

// 3. ENDPOINTS DEL PRODE (Rutas de la API)

#region SELECCIONES (EQUIPOS)
app.MapGet("/api/equipos", async (IEquipoRepository repo) => Results.Ok(await repo.ObtenerTodosAsync()));
app.MapPost("/api/equipos", async (CrearEquipoDTO datos, IEquipoRepository repo) =>
{
    try
    {
        var nuevoEquipo = new Equipo(datos.Nombre, datos.LogoUrl);
        await repo.GuardarAsync(nuevoEquipo);
        return Results.Created($"/api/equipos/{nuevoEquipo.Id}", nuevoEquipo);
    }
    catch (ArgumentException ex) { return Results.BadRequest(ex.Message); }
});
#endregion

#region USUARIOS
// Traer los competidores (Vos y tu amigo)
app.MapGet("/api/usuarios", async (IUsuarioRepository repo) => Results.Ok(await repo.ObtenerTodosAsync()));

// Crear un competidor
app.MapPost("/api/usuarios", async (CrearUsuarioDTO datos, IUsuarioRepository repo) =>
{
    try
    {
        // 🚀 Le pasamos las 4 cosas que nos pide el nuevo constructor:
        var nuevoUsuario = new Usuario(datos.Nombre, datos.Username, datos.Email, datos.Password);

        await repo.GuardarAsync(nuevoUsuario);
        return Results.Created($"/api/usuarios/{nuevoUsuario.Id}", nuevoUsuario);
    }
    catch (ArgumentException ex) { return Results.BadRequest(ex.Message); }
});
#endregion

#region FECHAS
// Traer el calendario (Fase de grupos, etc.)
app.MapGet("/api/fechas", async (IFechaRepository repo) => Results.Ok(await repo.ObtenerTodasAsync()));

// Crear una fecha/etapa
app.MapPost("/api/fechas", async (CrearFechaDTO datos, IFechaRepository repo) =>
{
    try
    {
        var nuevaFecha = new Fecha(datos.Nombre, datos.Orden);
        await repo.GuardarAsync(nuevaFecha);
        return Results.Created($"/api/fechas/{nuevaFecha.Id}", nuevaFecha);
    }
    catch (ArgumentException ex) { return Results.BadRequest(ex.Message); }
});
#endregion

#region PARTIDOS (EL FIXTURE REAL)
// Traer todos los partidos armados
app.MapGet("/api/partidos", async (IPartidoRepository repo) => Results.Ok(await repo.ObtenerTodosAsync()));

// Armar un cruce real de selecciones
app.MapPost("/api/partidos", async (CrearPartidoDTO datos, IPartidoRepository repo) =>
{
    try
    {
        var nuevoPartido = new Partido(datos.FechaId, datos.LocalId, datos.VisitanteId);
        await repo.GuardarAsync(nuevoPartido);
        return Results.Created($"/api/partidos/{nuevoPartido.Id}", nuevoPartido);
    }
    catch (ArgumentException ex) { return Results.BadRequest(ex.Message); }
});

// Cargar el resultado oficial cuando termine el partido de verdad
app.MapPost("/api/partidos/{id}/resultado", async (Guid id, RegistrarResultadoDTO datos, IPartidoRepository repo) =>
{
    var partido = await repo.ObtenerPorIdAsync(id);
    if (partido == null) return Results.NotFound("El partido no existe.");

    // Cambia el estado a finalizado y le clava los goles reales
    partido.RegistrarResultado(datos.GolesLocal, datos.GolesVisitante);
    await repo.ActualizarAsync(partido);

    // NOTA FUTURA: Acá es donde después le gatillamos el cálculo de puntos a las predicciones.
    return Results.Ok(partido);
});
#endregion

// ==========================================
// ENDPOINTS DE PREDICCIONES (A PRUEBA DE BALAS)
// ==========================================

// POST: Guardar o actualizar
app.MapPost("/api/predicciones", (Prediccion datos) =>
{
    try
    {
        // 1. Borramos si ya existía un voto del mismo usuario para el mismo partido
        predicciones.RemoveAll(p => p.UsuarioId == datos.UsuarioId && p.PartidoId == datos.PartidoId);

        // 2. Creamos la nueva predicción usando la entidad limpia de tu Dominio
        var nuevaPrediccion = new TorneosFutbol.Domain.Entities.Prediccion(datos.UsuarioId, datos.PartidoId, datos.GolesLocalVoto,datos.GolesVisitanteVoto);
        predicciones.Add(nuevaPrediccion);

        return Results.Ok(new { mensaje = "¡Pronóstico guardado exitosamente!", id = nuevaPrediccion.Id });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

// GET: Listar (Cambiamos el filtro a un formato plano que Swagger ama)
app.MapGet("/api/predicciones", ([FromQuery] Guid? uId) =>
{
    if (uId.HasValue)
    {
        var filtradas = predicciones.Where(p => p.UsuarioId == uId.Value).ToList();
        return Results.Ok(filtradas);
    }
    return Results.Ok(predicciones);
});

app.Run();

// DTOs para transferencia de datos limpia desde la web
public record CrearEquipoDTO(string Nombre, string LogoUrl);
public record CrearUsuarioDTO(string Nombre, string Username, string Email, string Password);
public record CrearFechaDTO(string Nombre, int Orden);
public record CrearPartidoDTO(Guid FechaId, Guid LocalId, Guid VisitanteId);
public record RegistrarResultadoDTO(int GolesLocal, int GolesVisitante);

public record GuardarPrediccionDTO(Guid UsuarioId, Guid PartidoId, int GolesLocalVoto, int GolesVisitanteVoto);