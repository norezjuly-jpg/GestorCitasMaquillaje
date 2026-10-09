using GestorCitasMaquillaje.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorCitasMaquillaje.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private static List<Cliente> clientes = new List<Cliente>
        {
            new Cliente { Id = 1, Nombre = "María López", Telefono = "809-555-0101", Email = "maria@email.com" },
            new Cliente { Id = 2, Nombre = "Ana Gómez", Telefono = "809-555-0202", Email = "ana@email.com" }
        };

        [HttpGet]
        public IActionResult Get() => Ok(clientes);

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null) return NotFound("Cliente no encontrado.");
            return Ok(cliente);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Cliente nuevoCliente)
        {
            nuevoCliente.Id = clientes.Max(c => c.Id) + 1;
            clientes.Add(nuevoCliente);
            return CreatedAtAction(nameof(Get), new { id = nuevoCliente.Id }, nuevoCliente);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Cliente clienteActualizado)
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null) return NotFound("Cliente no encontrado.");

            cliente.Nombre = clienteActualizado.Nombre;
            cliente.Telefono = clienteActualizado.Telefono;
            cliente.Email = clienteActualizado.Email;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null) return NotFound("Cliente no encontrado.");

            clientes.Remove(cliente);
            return NoContent();
        }
    }
}
