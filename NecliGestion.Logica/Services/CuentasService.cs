using NecliGestion.Entidades.Entidades;
using NecliGestion.Logica.Dtos;
using NecliGestion.Logica.Interfaces;
using NecliGestion.Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Services;


public class CuentaService : ICuentaService
{
    private readonly ICuentaRepository _cuentaRepo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly ICorreoService _correoService;


    public CuentaService(ICuentaRepository cuentaRepo, IUsuarioRepository usuarioRepo, ICorreoService correoService)
    {
        _cuentaRepo = cuentaRepo;
        _usuarioRepo = usuarioRepo;
        _correoService = correoService;
    }

    public Cuenta CrearCuenta(CrearCuentaDto dto)
    {
        var hoy = DateTime.Today;
        var edad = hoy.Year - dto.FechaNacimiento.Year;
        if (dto.FechaNacimiento.Date > hoy.AddYears(-edad)) edad--;

        if (edad < 15)
            throw new InvalidOperationException("El usuario debe ser mayor de 15 años para crear una cuenta.");

        if (string.IsNullOrWhiteSpace(dto.Nombres) || string.IsNullOrWhiteSpace(dto.Apellidos))
            throw new ArgumentException("Nombres y apellidos son requeridos");

        if (!Regex.IsMatch(dto.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Correo inválido");

        if (dto.Telefono.Length != 10 || !dto.Telefono.All(char.IsDigit))
            throw new ArgumentException("El teléfono debe tener 10 dígitos");

        if (_usuarioRepo.GetByIdentificacion(dto.Identificacion) != null)
            throw new InvalidOperationException("Usuario ya registrado");

        if (_cuentaRepo.GetByTelefono(dto.Telefono) != null)
            throw new InvalidOperationException("Número de cuenta ya existe");

        var contrasenaHash = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);

        // Generar token único para verificación
        var token = Guid.NewGuid().ToString();

        var usuario = new Usuario
        {
            Identificacion = dto.Identificacion,
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            Correo = dto.Correo,
            Telefono = dto.Telefono,
            Contrasena = contrasenaHash,
            FechaNacimiento = dto.FechaNacimiento,
            TipoUsuario = "Persona Natural",
            EsCorreoVerificado = false,
            TokenVerificacionCorreo = token,
            FechaExpiracionToken = DateTime.UtcNow.AddHours(24) // Expira en 24h
        };

        var saldoInicial = 0.0m;
        var cuenta = new Cuenta
        {
            Telefono = dto.Telefono,
            FechaCreacion = DateTime.Now,
            Saldo = saldoInicial,
            Usuario = usuario,
            Nombres = dto.Nombres
        };

        _cuentaRepo.Create(cuenta);

        // Enviar correo con link de verificación
        var linkVerificacion = $"https://localhost:7285/api/cuentas/verificarcorreo?correo={Uri.EscapeDataString(usuario.Correo)}&token={token}";
        var contenidoCorreo = $"<p>Hola {usuario.Nombres},</p>" +
            $"<p>Para confirmar tu correo, haz clic en el siguiente enlace:</p>" +
            $"<a href='{linkVerificacion}'>Confirmar Correo</a>" +
            $"<p>Este enlace expirará en 24 horas.</p>"+
            $"<p><strong>Token de verificación:</strong> {token}</p>";

        _correoService.EnviarCorreo(usuario.Correo, "Confirma tu correo electrónico", contenidoCorreo);

        return cuenta;
    }

    public void ConfirmarCorreo(VerificarCorreoDto dto)
    {
        var usuario = _usuarioRepo.GetByCorreo(dto.Correo);
        if (usuario == null)
            throw new InvalidOperationException("Correo no registrado");

        if (usuario.EsCorreoVerificado)
            throw new InvalidOperationException("Correo ya confirmado");

        if (usuario.TokenVerificacionCorreo != dto.Token)
            throw new InvalidOperationException("Token de verificación inválido");

        if (!usuario.FechaExpiracionToken.HasValue || usuario.FechaExpiracionToken < DateTime.UtcNow)
            throw new InvalidOperationException("Token de verificación expirado");

        usuario.EsCorreoVerificado = true;
        usuario.TokenVerificacionCorreo = null;
        usuario.FechaExpiracionToken = null;

        _usuarioRepo.Actualizar(usuario);

        // Enviar correo de confirmación final
        var contenidoFinal = $"<p>Hola {usuario.Nombres},</p><p>Tu correo ha sido confirmado exitosamente. ¡Bienvenido!</p>";
        _correoService.EnviarCorreo(usuario.Correo, "Correo confirmado", contenidoFinal);
    }


    public ObtenerCuentaDto ObtenerCuenta(string telefono)
    {
        var cuenta = _cuentaRepo.GetByTelefono(telefono);
        if (cuenta == null)
            throw new KeyNotFoundException("Cuenta no encontrada");

        return new ObtenerCuentaDto(
            Telefono: cuenta.Telefono,
            Nombres: cuenta.Usuario.Nombres,
            Apellidos: cuenta.Usuario.Apellidos,
            Correo: cuenta.Usuario.Correo,
            Saldo: cuenta.Saldo,
            FechaCreacion: cuenta.FechaCreacion
        );
    }


    public void EliminarCuenta(string telefono)
    {
        var cuenta = _cuentaRepo.GetByTelefono(telefono);
        if (cuenta == null)
            throw new KeyNotFoundException("Cuenta no encontrada");

        if (cuenta.Saldo > 50000)
            throw new InvalidOperationException("No se puede eliminar una cuenta con más de $50,000");

        _cuentaRepo.Delete(cuenta);
    }
    public List<ObtenerCuentaDto> ObtenerCuentas()
    {
        var cuentas = _cuentaRepo.GetAll(); 

        return cuentas.Select(c => new ObtenerCuentaDto(
            Telefono: c.Telefono,
            Nombres: c.Usuario.Nombres,
            Apellidos: c.Usuario.Apellidos,
            Correo: c.Usuario.Correo,
            Saldo: c.Saldo,
            FechaCreacion: c.FechaCreacion
        )).ToList();
    }

}

