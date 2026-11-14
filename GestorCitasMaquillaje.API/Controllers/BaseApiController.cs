using GestorCitasMaquillaje.Application.Core;
using Microsoft.AspNetCore.Mvc;

namespace GestorCitasMaquillaje.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult Responder(ServiceResult result)
        {
            if (result.Success)
                return Ok(result);

            if (result.Message.Contains("no existe", StringComparison.OrdinalIgnoreCase))
                return NotFound(result);

            return BadRequest(result);
        }
    }
}
