using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NecliGestion.Entidades.Entidades;
using NecliGestion.Logica.Dtos;
using NecliGestion.Logica.Interfaces;

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

    [HttpPost]
    public ActionResult<TransaccionResultadoDto> RealizarTransaccion([FromBody] TransaccionDto dto)
    {
        try
        {
            var resultado = _transaccionService.RealizarTransaccion(dto);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{telefono}")]
    public ActionResult<List<ObtenerTransaccionDto>> ConsultarTransacciones(string telefono,[FromQuery] DateTime? desde,[FromQuery] DateTime? hasta)
    {
        return Ok(_transaccionService.ConsultarTransacciones(telefono, desde, hasta));
    }
}
