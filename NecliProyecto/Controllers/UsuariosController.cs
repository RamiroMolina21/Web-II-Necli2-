﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NecliGestion.Entidades.Entidades;
using NecliGestion.Logica.Dtos;
using NecliGestion.Logica.Interfaces;

namespace NecliProyecto.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }


    [Authorize]
    [HttpGet("{identificacion}")]
    public ActionResult<ObtenerUsuarioDto> ObtenerUsuario(string identificacion)
    {
        try
        {
            return Ok(_usuarioService.ObtenerUsuario(identificacion));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


    [Authorize]
    [HttpPut("{identificacion}")]
    public IActionResult ActualizarUsuario(string identificacion, [FromBody] ActualizarUsuarioDto dto)
    {
        try
        {
            _usuarioService.ActualizarUsuario(identificacion, dto);
            return Ok("La cuenta ha sido actualizada correctamente");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [Authorize]
    [HttpGet]
    public ActionResult<List<ObtenerUsuarioDto>> ObtenerUsuarios()
    {
        return Ok(_usuarioService.ObtenerUsuarios());
    }
}
