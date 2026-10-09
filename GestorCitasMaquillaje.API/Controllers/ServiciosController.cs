using GestorCitasMaquillaje.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorCitasMaquillaje.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private static List<Servicio> servicios = new List<Servicio>
        {
            new Servicio { Id = 1, Nombre = "Maquillaje Social", Precio = 2500, DuracionMinutos = 60 },
            new Servicio { Id = 2, Nombre = "Maquillaje de Novia", Precio = 5000, DuracionMinutos = 90 }
        };

        [HttpGet]
        public IActionResult Get() => Ok(servicios);

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var servicio = servicios.FirstOrDefault(s => s.Id == id);
            if (servicio == null) return NotFound("Servicio no encontrado.");
            return Ok(servicio);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Servicio nuevoServicio)
        {
            nuevoServicio.Id = servicios.Max(s => s.Id) + 1;
            servicios.Add(nuevoServicio);
            return CreatedAtAction(nameof(Get), new { id = nuevoServicio.Id }, nuevoServicio);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Servicio servicioActualizado)
        {
            var servicio = servicios.FirstOrDefault(s => s.Id == id);
            if (servicio == null) return NotFound("Servicio no encontrado.");

            servicio.Nombre = servicioActualizado.Nombre;
            servicio.Precio = servicioActualizado.Precio;
            servicio.DuracionMinutos = servicioActualizado.DuracionMinutos;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var servicio = servicios.FirstOrDefault(s => s.Id == id);
            if (servicio == null) return NotFound("Servicio no encontrado.");

            servicios.Remove(servicio);
            return NoContent();
        }
    }
}
