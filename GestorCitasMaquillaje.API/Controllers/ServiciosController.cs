using GestorCitasMaquillaje.Application.Contract;
using GestorCitasMaquillaje.Application.Dtos;
using GestorCitasMaquillaje.Application.Dtos.Servicio;
using Microsoft.AspNetCore.Mvc;

namespace GestorCitasMaquillaje.API.Controllers;

public class ServiciosController : BaseApiController
{
    private readonly IServicioService _service;

    public ServiciosController(IServicioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Responder(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => Responder(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearServicioDto dto) =>
        Responder(await _service.CreateAsync(dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ActualizarServicioDto dto)
    {
        dto.Id = id;
        return Responder(await _service.UpdateAsync(dto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => Responder(await _service.DeleteAsync(id));
}