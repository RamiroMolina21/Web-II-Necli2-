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
    private readonly ICorreoService _correoService;

    public UsuarioService(IUsuarioRepository usuarioRepo, ICuentaRepository cuentaRepo, ICorreoService correoService)
    {
        _usuarioRepo = usuarioRepo;
        _cuentaRepo = cuentaRepo;
        _correoService = correoService;
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
            // Verificar contraseña anterior
            if (string.IsNullOrWhiteSpace(dto.ContrasenaAnterior))
                throw new ArgumentException("Se requiere la contraseña anterior para realizar el cambio");

            if (!BCrypt.Net.BCrypt.Verify(dto.ContrasenaAnterior, usuario.Contrasena))
                throw new ArgumentException("La contraseña anterior es incorrecta");

            // Actualizar contraseña
            usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);

            // Enviar correo de notificación
            var contenidoCorreo = $@"
                <h2>Cambio de Contraseña</h2>
                <p>Hola {usuario.Nombres},</p>
                <p>Tu contraseña ha sido actualizada exitosamente.</p>
                <p>Si no realizaste este cambio, por favor contacta a soporte inmediatamente.</p>
                <p>Fecha y hora del cambio: {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>";

            _correoService.EnviarCorreo(
                usuario.Correo,
                "Cambio de Contraseña - Necli Gestión",
                contenidoCorreo
            );
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

    public Usuario Autenticar(string identificacion, string contrasena)
    {
        var usuario = _usuarioRepo.GetByIdentificacion(identificacion);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(contrasena, usuario.Contrasena))
            return null;

        return usuario;
    }



}
