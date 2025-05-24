using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NecliGestion.Entidades.Entidades;
using NecliGestion.Logica.Dtos;
using NecliGestion.Logica.Interfaces;

namespace NecliProyecto.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CuentaController : ControllerBase
{
    private readonly ICuentaService _cuentaService;

    public CuentaController(ICuentaService cuentaService)
    {
        _cuentaService = cuentaService;
    }

    [HttpPost]
    public ActionResult<CrearCuentaDto> CrearCuenta([FromBody] CrearCuentaDto dto)
    {
        try
        {
            var cuenta = _cuentaService.CrearCuenta(dto);
            return CreatedAtAction(nameof(ObtenerCuenta), new { telefono = cuenta.Telefono }, cuenta);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{telefono}")]
    public ActionResult<ObtenerCuentaDto> ObtenerCuenta(string telefono)
    {
        try
        {
            return Ok(_cuentaService.ObtenerCuenta(telefono));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{telefono}")]
    public IActionResult EliminarCuenta(string telefono)
    {
        try
        {
            _cuentaService.EliminarCuenta(telefono);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public ActionResult<List<ObtenerCuentaDto>> ObtenerCuentas()
    {
        return Ok(_cuentaService.ObtenerCuentas());
    }
}