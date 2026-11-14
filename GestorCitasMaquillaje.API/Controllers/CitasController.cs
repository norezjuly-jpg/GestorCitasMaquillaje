using GestorCitasMaquillaje.Application.Contract;
using GestorCitasMaquillaje.Application.Dtos;
using GestorCitasMaquillaje.Application.Dtos.Cita;
using Microsoft.AspNetCore.Mvc;

namespace GestorCitasMaquillaje.API.Controllers;

public class CitasController : BaseApiController
{
    private readonly ICitaService _service;

    public CitasController(ICitaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Responder(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => Responder(await _service.GetByIdAsync(id));

    [HttpGet("cliente/{clienteId}")]
    public async Task<IActionResult> GetByCliente(int clienteId) =>
        Responder(await _service.GetByClienteAsync(clienteId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearCitaDto dto) =>
        Responder(await _service.CreateAsync(dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ActualizarCitaDto dto)
    {
        dto.Id = id;
        return Responder(await _service.UpdateAsync(dto));
    }

    [HttpPut("{id}/cancelar")]
    public async Task<IActionResult> Cancelar(int id, [FromBody] CancelarCitaDto dto)
    {
        dto.Id = id;
        return Responder(await _service.CancelarAsync(dto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => Responder(await _service.DeleteAsync(id));
}