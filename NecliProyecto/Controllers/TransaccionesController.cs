using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NecliGestion.Entidades.Entidades;
using NecliGestion.Logica.Dtos;
using NecliGestion.Logica.Exceptions;
using NecliGestion.Logica.Interfaces;
using System.Security.Claims;

namespace NecliProyecto.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransaccionController : ControllerBase
{
    private readonly ITransaccionService _transaccionService;

    public TransaccionController(ITransaccionService transaccionService)
    {
        _transaccionService = transaccionService;
    }

    [Authorize]
    [HttpPost("Nequi y Bancolombia")]
    public ActionResult<TransaccionResultadoDto> RealizarTransaccion([FromBody] TransaccionDto dto)
    {
        try
        {
            // Obtener el usuarioId del token
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized("Usuario no autenticado");

            var resultado = _transaccionService.RealizarTransaccion(usuarioId, dto);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [Authorize]
    [HttpGet("{telefono}")]
    public ActionResult<List<ObtenerTransaccionDto>> ConsultarTransacciones(string telefono,[FromQuery] DateTime? desde,[FromQuery] DateTime? hasta)
    {
        return Ok(_transaccionService.ConsultarTransacciones(telefono, desde, hasta));
    }

    [Authorize]
    [HttpPost("Otros Bancos")]
    public async Task<IActionResult> RealizarTransaccionInterbancaria([FromBody] TransaccionInterbancariaDto dto)
    {
        try
        {
            var usuarioIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (usuarioIdClaim == null) throw new UnauthorizedAccessException("User not authenticated.");

            var usuarioId = usuarioIdClaim.Value;

            var resultado = await _transaccionService.RealizarTransaccionInterbancaria(usuarioId, dto);
            if (resultado)
            {
                return Ok("Transacción interbancaria realizada con éxito.");
            }
            else
            {
                return BadRequest("No se pudo realizar la transacción interbancaria.");
            }
        }
        catch (NegocioException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error al procesar la transacción.");
        }
    }
}
