using GestorCitasMaquillaje.API.Data;
using GestorCitasMaquillaje.API.DTOs;
using GestorCitasMaquillaje.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestorCitasMaquillaje.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiciosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServiciosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioDto>>> GetAll()
        {
            var lista = await _context.Servicios
                .Select(s => new ServicioDto
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    Precio = s.Precio,
                    DuracionMinutos = s.DuracionMinutos
                }).ToListAsync();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioDto>> GetById(int id)
        {
            var s = await _context.Servicios.FindAsync(id);
            if (s == null) return NotFound();
            return Ok(new ServicioDto
            {
                Id = s.Id,
                Nombre = s.Nombre,
                Precio = s.Precio,
                DuracionMinutos = s.DuracionMinutos
            });
        }

        [HttpPost]
        public async Task<ActionResult<ServicioDto>> Create(ServicioCreateDto dto)
        {
            var s = new Servicio
            {
                Nombre = dto.Nombre,
                Precio = dto.Precio,
                DuracionMinutos = dto.DuracionMinutos
            };
            _context.Servicios.Add(s);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = s.Id },
                new ServicioDto { Id = s.Id, Nombre = s.Nombre, Precio = s.Precio, DuracionMinutos = s.DuracionMinutos });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ServicioCreateDto dto)
        {
            var s = await _context.Servicios.FindAsync(id);
            if (s == null) return NotFound();
            s.Nombre = dto.Nombre;
            s.Precio = dto.Precio;
            s.DuracionMinutos = dto.DuracionMinutos;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var s = await _context.Servicios.FindAsync(id);
            if (s == null) return NotFound();
            _context.Servicios.Remove(s);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}