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
        var nuevoEquipo = new Equipo(datos.Nombre, datos.LogoUrl, datos.Grupo);
        await repo.GuardarAsync(nuevoEquipo);
        return Results.Created($"/api/equipos/{nuevoEquipo.Id}", nuevoEquipo);
    }
    catch (ArgumentException ex) { return Results.BadRequest(ex.Message); }
});
#endregion

#region USUARIOS

// POST: Login / Registro con Google
app.MapPost("/api/usuarios/google", async ([FromBody] GuardarGoogleUserDTO datos, IUsuarioRepository repo) =>
{
    try
    {
        // 1. Buscamos todos los usuarios para ver si el correo de Google ya existe
        var usuarios = await repo.ObtenerTodosAsync();
        var usuarioExistente = usuarios.FirstOrDefault(u =>
            u.Email.Equals(datos.Email, StringComparison.OrdinalIgnoreCase));

        // 2. Si el usuario NO existe, lo registramos automáticamente de forma silenciosa
        if (usuarioExistente == null)
        {
            // Creamos un username único basado en su mail o nombre
            string usernameAutomatico = datos.Email.Split('@')[0] + "_" + Guid.NewGuid().ToString().Substring(0, 4);

            // Le ponemos una contraseña aleatoria y segura por defecto (ya que entra por Google)
            string passwordSegura = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString());

            usuarioExistente = new Usuario(datos.Nombre, usernameAutomatico, datos.Email, passwordSegura);
            await repo.GuardarAsync(usuarioExistente);
        }

        // 3. Devolvemos los datos del usuario logueado exitosamente
        return Results.Ok(new
        {
            id = usuarioExistente.Id,
            nombre = usuarioExistente.Nombre,
            username = usuarioExistente.Username,
            email = usuarioExistente.Email
        });
    }
    catch (Exception ex)
    {
        return Results.Problem("Hubo un error al sincronizar con Google: " + ex.Message);
    }
});

// POST: Login de usuarios
app.MapPost("/api/usuarios/login", async (LoginDTO datos, IUsuarioRepository repo) =>
{
    try
    {
        // 1. Buscamos todos los usuarios para filtrar (o si tenés un método en tu repo que busque por username/email usás ese)
        var usuarios = await repo.ObtenerTodosAsync();

        // Buscamos coincidencia ignorando mayúsculas/minúsculas tal cual tu consulta de Supabase
        var usuarioExistente = usuarios.FirstOrDefault(u =>
            u.Username.Equals(datos.InputUsuario, StringComparison.OrdinalIgnoreCase) ||
            u.Email.Equals(datos.InputUsuario, StringComparison.OrdinalIgnoreCase));

        // 2. Si no existe el usuario, rebotamos
        if (usuarioExistente == null)
        {
            return Results.BadRequest("Usuario o contraseña incorrectos.");
        }

        // 3. 🔥 VERIFICAMOS LA CONTRASEÑA CON BCRYPT
        // Compara la clave en texto plano del frontend con el Hash seguro de la base de datos
        bool esValida = BCrypt.Net.BCrypt.Verify(datos.Password, usuarioExistente.PasswordHash);

        if (!esValida)
        {
            return Results.BadRequest("Usuario o contraseña incorrectos.");
        }

        // 4. Si todo está ok, le devolvemos los datos del usuario (menos el Hash por seguridad)
        return Results.Ok(new
        {
            id = usuarioExistente.Id,
            nombre = usuarioExistente.Nombre,
            username = usuarioExistente.Username,
            email = usuarioExistente.Email
        });
    }
    catch (Exception ex)
    {
        return Results.Problem("Hubo un error al procesar el ingreso: " + ex.Message);
    }
});

// Traer los competidores (Vos y tu amigo)
app.MapGet("/api/usuarios", async (IUsuarioRepository repo) => Results.Ok(await repo.ObtenerTodosAsync()));

// Crear un competidor
app.MapPost("/api/usuarios", async (CrearUsuarioDTO datos, IUsuarioRepository repo) =>
{
    try
    {
        string contraseñaEncriptada = BCrypt.Net.BCrypt.HashPassword(datos.Password);
        // 🚀 Le pasamos las 4 cosas que nos pide el nuevo constructor:
        var nuevoUsuario = new Usuario(datos.Nombre, datos.Username, datos.Email, contraseñaEncriptada);

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
        var nuevoPartido = new Partido(datos.FechaId, datos.LocalId, datos.VisitanteId, datos.Fecha, datos.Hora);
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
app.MapPost("/api/predicciones", (GuardarPrediccionDTO datos) =>
{
    try
    {
        // 1. Borramos si ya existía un voto del mismo usuario para el mismo partido
        predicciones.RemoveAll(p => p.UsuarioId == datos.UsuarioId && p.PartidoId == datos.PartidoId && p.TorneoId == datos.TorneoId);

        // 2. Creamos la nueva predicción usando la entidad limpia de tu Dominio
        var nuevaPrediccion = new TorneosFutbol.Domain.Entities.Prediccion(datos.UsuarioId, datos.PartidoId, datos.GolesLocalVoto,datos.GolesVisitanteVoto);

        nuevaPrediccion.TorneoId = datos.TorneoId;

        predicciones.Add(nuevaPrediccion);

        return Results.Ok(new { mensaje = "¡Pronóstico guardado exitosamente!", id = nuevaPrediccion.Id });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});


app.MapPost("/api/torneos", async (CrearTorneoDTO datos, AppDbContext db) =>
{
    try
    {
        // IMPRIMÍ EN CONSOLA PARA VER QUÉ ESTÁ PASANDO
        Console.WriteLine($"RECIBIDO CREATORID: {datos.CreadorId}");

        var usuarioExiste = await db.Usuarios.AnyAsync(u => u.Id == datos.CreadorId);

        if (!usuarioExiste)
        {
            // Esto te dirá si el ID que llega es realmente el que crees
            return Results.BadRequest(new
            {
                error = "Usuario no encontrado",
                receivedId = datos.CreadorId
            });
        }

        var nuevoTorneo = new Torneo
        {
            Id = Guid.NewGuid(),
            NombreTorneo = datos.NombreTorneo,
            CreadorId = datos.CreadorId,
            CreatedAt = DateTime.UtcNow,
            TokenAcceso = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()
        };

        db.Torneos.Add(nuevoTorneo);
        await db.SaveChangesAsync();

        return Results.Ok(nuevoTorneo);
    }
    catch (Exception ex)
    {
        var error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        return Results.BadRequest(new { error = error });
    }
});

app.MapPost("/api/torneos/unirse/{token}", (string token, Guid usuarioId, AppDbContext db) =>
{
    // 1. Buscamos el torneo usando el token (asegúrate de que TokenAcceso exista en tu entidad Torneo)
    var torneo = db.Torneos.FirstOrDefault(t => t.TokenAcceso == token);

    if (torneo == null)
    {
        return Results.NotFound(new { mensaje = "Torneo no encontrado." });
    }

    // 2. Verificamos si ya existe la relación para evitar errores de clave duplicada
    bool yaEstaUnido = db.Set<TorneoParticipante>().Any(tp =>
        tp.TorneoId == torneo.Id &&
        tp.UsuarioId == usuarioId);

    if (yaEstaUnido)
    {
        return Results.BadRequest(new { mensaje = "Ya sos parte de este torneo." });
    }

    // 3. Creamos la relación
    var nuevoParticipante = new TorneoParticipante(torneo.Id, usuarioId);

    db.Set<TorneoParticipante>().Add(nuevoParticipante);

    // 4. Guardamos cambios
    db.SaveChanges(); // O SaveChangesAsync()

    return Results.Ok(new { mensaje = $"¡Te uniste al torneo {torneo.NombreTorneo} exitosamente!" });
});

app.MapGet("/api/predicciones", ([FromQuery] Guid? uId, [FromQuery] Guid? torneoId, AppDbContext db) =>
{
    // Empezamos con la consulta base a la tabla de Predicciones
    var query = db.Predicciones.AsQueryable();

    // Filtramos por Usuario si nos lo pasan
    if (uId.HasValue)
    {
        query = query.Where(p => p.UsuarioId == uId.Value);
    }

    // FILTRO CRÍTICO: Filtramos por el TorneoId
    // Si torneoId es NULL, asumimos que son las predicciones globales (o las que no pertenecen a torneos)
    // Si nos pasan un torneoId, traemos solo las de ese torneo
    query = query.Where(p => p.TorneoId == torneoId);

    return Results.Ok(query.ToList());
});

app.MapGet("/api/torneos/{torneoId}/posiciones", (Guid torneoId, AppDbContext db) =>
{
    var posiciones = db.Predicciones
        .Where(p => p.TorneoId == torneoId)
        .GroupBy(p => p.UsuarioId)
        .Select(g => new
        {
            UsuarioId = g.Key,
            // Traemos el nombre del usuario buscando en la tabla Usuarios
            NombreUsuario = db.Usuarios
                .Where(u => u.Id == g.Key)
                .Select(u => u.Nombre)
                .FirstOrDefault(),
            TotalPuntos = g.Sum(p => p.PuntosGanados)
        })
        .OrderByDescending(p => p.TotalPuntos)
        .ToList();

    return Results.Ok(posiciones);
});

app.MapPost("/api/partidos/{partidoId}/resultado", async (Guid partidoId, RegistrarResultadoDTO resultado, AppDbContext db) =>
{
    var partido = await db.Partidos.FindAsync(partidoId);
    if (partido == null) return Results.NotFound("Partido no encontrado.");

    // 1. Actualizamos el resultado real
    partido.RegistrarResultado(resultado.GolesLocal, resultado.GolesVisitante);

    // 2. Buscamos todas las predicciones de este partido
    var predicciones = db.Predicciones.Where(p => p.PartidoId == partidoId).ToList();

    // 3. Calculamos puntos para cada una
    foreach (var pred in predicciones)
    {
        pred.CalcularPuntos(partido); // ¡Acá reutilizamos tu lógica de dominio!
    }

    await db.SaveChangesAsync();

    return Results.Ok(new { mensaje = "Resultado cargado y puntos recalculados." });
});

app.Run();

// DTOs para transferencia de datos limpia desde la web
public record CrearEquipoDTO(string Nombre, string LogoUrl, string Grupo);
public record CrearUsuarioDTO(string Nombre, string Username, string Email, string Password);
public record CrearFechaDTO(string Nombre, int Orden);
public record CrearPartidoDTO(Guid FechaId, Guid LocalId, Guid VisitanteId, string Fecha, string Hora);
public record RegistrarResultadoDTO(int GolesLocal, int GolesVisitante);
public record GuardarPrediccionDTO(Guid UsuarioId, Guid PartidoId, int GolesLocalVoto, int GolesVisitanteVoto, Guid? TorneoId);
public record LoginDTO(string InputUsuario, string Password); // InputUsuario puede ser el Email o el Username
public record GuardarGoogleUserDTO(string Nombre, string Email);
public record CrearTorneoDTO(string NombreTorneo, Guid CreadorId);