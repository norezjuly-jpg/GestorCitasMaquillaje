using GestorCitasMaquillaje.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorCitasMaquillaje.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private static List<Cita> citas = new List<Cita>
        {
            new Cita { Id = 1, ClienteId = 1, MaquillistaId = 1, ServicioId = 1, FechaHora = DateTime.Now.AddDays(1), Estado = "Confirmada" }
        };

        [HttpGet]
        public IActionResult Get() => Ok(citas);

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var cita = citas.FirstOrDefault(c => c.Id == id);
            if (cita == null) return NotFound("Cita no encontrada.");
            return Ok(cita);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Cita nuevaCita)
        {
            nuevaCita.Id = citas.Max(c => c.Id) + 1;
            citas.Add(nuevaCita);
            return CreatedAtAction(nameof(Get), new { id = nuevaCita.Id }, nuevaCita);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Cita citaActualizada)
        {
            var cita = citas.FirstOrDefault(c => c.Id == id);
            if (cita == null) return NotFound("Cita no encontrada.");

            cita.ClienteId = citaActualizada.ClienteId;
            cita.MaquillistaId = citaActualizada.MaquillistaId;
            cita.ServicioId = citaActualizada.ServicioId;
            cita.FechaHora = citaActualizada.FechaHora;
            cita.Estado = citaActualizada.Estado;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cita = citas.FirstOrDefault(c => c.Id == id);
            if (cita == null) return NotFound("Cita no encontrada.");

            citas.Remove(cita);
            return NoContent();
        }
    }
}
