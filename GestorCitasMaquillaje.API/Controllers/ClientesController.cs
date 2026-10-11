using GestorCitasMaquillaje.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestorCitasMaquillaje.API.Data;
using GestorCitasMaquillaje.API.DTOs;

namespace GestorCitasMaquillaje.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetAll()
        {
            var lista = await _context.Clientes
                .Select(c => new ClienteDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Telefono = c.Telefono,
                    Email = c.Email
                }).ToListAsync();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> GetById(int id)
        {
            var c = await _context.Clientes.FindAsync(id);
            if (c == null) return NotFound();
            return Ok(new ClienteDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Telefono = c.Telefono,
                Email = c.Email
            });
        }

        [HttpPost]
        public async Task<ActionResult<ClienteDto>> Create(ClienteCreateDto dto)
        {
            var c = new Cliente
            {
                Nombre = dto.Nombre,
                Telefono = dto.Telefono,
                Email = dto.Email
            };
            _context.Clientes.Add(c);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = c.Id },
                new ClienteDto { Id = c.Id, Nombre = c.Nombre, Telefono = c.Telefono, Email = c.Email });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ClienteCreateDto dto)
        {
            var c = await _context.Clientes.FindAsync(id);
            if (c == null) return NotFound();
            c.Nombre = dto.Nombre;
            c.Telefono = dto.Telefono;
            c.Email = dto.Email;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _context.Clientes.FindAsync(id);
            if (c == null) return NotFound();
            _context.Clientes.Remove(c);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}