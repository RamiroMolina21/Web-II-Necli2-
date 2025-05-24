using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NecliGestion.Logica.Dtos;
using NecliGestion.Logica.Interfaces;

namespace NecliProyecto.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly ITokenService _tokenService;

    public AuthController(IUsuarioService usuarioService, ITokenService tokenService)
    {
        _usuarioService = usuarioService;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var usuario = _usuarioService.Autenticar(dto.Identificacion, dto.Contrasena);

        if (usuario == null)
            return Unauthorized("Credenciales inválidas");

        var token = _tokenService.GenerarToken(usuario);

        return Ok(new { token });
    }
}
