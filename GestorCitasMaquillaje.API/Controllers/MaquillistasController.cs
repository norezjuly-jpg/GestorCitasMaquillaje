using GestorCitasMaquillaje.Application.Contract;
using GestorCitasMaquillaje.Application.Dtos;
using GestorCitasMaquillaje.Application.Dtos.Maquillista;
using Microsoft.AspNetCore.Mvc;

namespace GestorCitasMaquillaje.API.Controllers;

public class MaquillistasController : BaseApiController
{
    private readonly IMaquillistaService _service;

    public MaquillistasController(IMaquillistaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Responder(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => Responder(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearMaquillistaDto dto) =>
        Responder(await _service.CreateAsync(dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ActualizarMaquillistaDto dto)
    {
        dto.Id = id;
        return Responder(await _service.UpdateAsync(dto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => Responder(await _service.DeleteAsync(id));
}