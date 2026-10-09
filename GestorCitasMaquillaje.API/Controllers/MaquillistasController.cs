using GestorCitasMaquillaje.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorCitasMaquillaje.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaquillistasController : ControllerBase
    {
        private static List<Maquillista> maquillistas = new List<Maquillista>
        {
            new Maquillista { Id = 1, Nombre = "Laura Martínez", Especialidad = "Maquillaje Social y Novias", Disponible = true },
            new Maquillista { Id = 2, Nombre = "Carla Pérez", Especialidad = "Maquillaje de Caracterización", Disponible = true }
        };

        [HttpGet]
        public IActionResult Get() => Ok(maquillistas);

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var maquillista = maquillistas.FirstOrDefault(m => m.Id == id);
            if (maquillista == null) return NotFound("Maquillista no encontrada.");
            return Ok(maquillista);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Maquillista nuevaMaquillista)
        {
            nuevaMaquillista.Id = maquillistas.Max(m => m.Id) + 1;
            maquillistas.Add(nuevaMaquillista);
            return CreatedAtAction(nameof(Get), new { id = nuevaMaquillista.Id }, nuevaMaquillista);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Maquillista maquillistaActualizada)
        {
            var maquillista = maquillistas.FirstOrDefault(m => m.Id == id);
            if (maquillista == null) return NotFound("Maquillista no encontrada.");

            maquillista.Nombre = maquillistaActualizada.Nombre;
            maquillista.Especialidad = maquillistaActualizada.Especialidad;
            maquillista.Disponible = maquillistaActualizada.Disponible;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var maquillista = maquillistas.FirstOrDefault(m => m.Id == id);
            if (maquillista == null) return NotFound("Maquillista no encontrada.");

            maquillistas.Remove(maquillista);
            return NoContent();
        }
    }
}
