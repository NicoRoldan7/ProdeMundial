using Microsoft.AspNetCore.Mvc;
using TorneosFutbol.Domain.Ports.Out; // Tus puertos de salida

namespace TorneosFutbol.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidosController : ControllerBase
    {
        private readonly IPartidoRepository _partidoRepository;

        // .NET inyecta el repositorio automáticamente gracias a la interfaz
        public PartidosController(IPartidoRepository partidoRepository)
        {
            _partidoRepository = partidoRepository;
        }

        // GET: api/partidos
        [HttpGet]
        public async Task<IActionResult> GetPartidos()
        {
            var partidos = await _partidoRepository.ObtenerTodosAsync();
            return Ok(partidos);
        }
    }
}