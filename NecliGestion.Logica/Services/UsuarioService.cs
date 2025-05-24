using NecliGestion.Entidades.Entidades;
using NecliGestion.Logica.Dtos;
using NecliGestion.Logica.Interfaces;
using NecliGestion.Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly ICuentaRepository _cuentaRepo;

    public UsuarioService(IUsuarioRepository usuarioRepo, ICuentaRepository cuentaRepo)
    {
        _usuarioRepo = usuarioRepo;
        _cuentaRepo = cuentaRepo;
    }

    public ObtenerUsuarioDto ObtenerUsuario(string identificacion)
    {
        var usuario = _usuarioRepo.GetByIdentificacion(identificacion);
        if (usuario == null)
            throw new KeyNotFoundException("Usuario no encontrado");

        return new ObtenerUsuarioDto(
            Identificacion: usuario.Identificacion,
            Nombres: usuario.Nombres,
            Apellidos: usuario.Apellidos,
            Correo: usuario.Correo,
            Telefono: usuario.Telefono
        );
    }


    public void ActualizarUsuario(string identificacion, ActualizarUsuarioDto dto)
    {
        var usuario = _usuarioRepo.GetByIdentificacion(identificacion);
        if (usuario == null)
            throw new KeyNotFoundException("Usuario no encontrado");

        if (!string.IsNullOrWhiteSpace(dto.Correo) &&
            dto.Correo != usuario.Correo &&
            !Regex.IsMatch(dto.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            throw new ArgumentException("Correo inválido");
        }

      
        usuario.Nombres = dto.Nombres;
        usuario.Apellidos = dto.Apellidos;
        usuario.Correo = dto.Correo;

        if (!string.IsNullOrWhiteSpace(dto.Contrasena))
        {
            usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);
        }

        _usuarioRepo.SaveChanges();

        var cuenta = _cuentaRepo.GetByTelefono(usuario.Telefono);
        if (cuenta != null && usuario.Telefono != cuenta.Telefono)
        {
            cuenta.Telefono = usuario.Telefono;
            _cuentaRepo.SaveChanges();
        }
    }

    public List<ObtenerUsuarioDto> ObtenerUsuarios()
    {
        var usuarios = _usuarioRepo.GetAll();
        return usuarios.Select(u => new ObtenerUsuarioDto(
            u.Identificacion,
            u.Nombres,
            u.Apellidos,
            u.Correo,
            u.Telefono
        )).ToList();
    }


}
