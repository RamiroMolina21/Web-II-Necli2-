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

    public CuentaService(ICuentaRepository cuentaRepo, IUsuarioRepository usuarioRepo)
    {
        _cuentaRepo = cuentaRepo;
        _usuarioRepo = usuarioRepo;
    }

    public Cuenta CrearCuenta(CrearCuentaDto dto)
    {
        var hoy = DateTime.Today;
        var edad = hoy.Year - dto.FechaNacimiento.Year;
        if (dto.FechaNacimiento.Date > hoy.AddYears(-edad))
        {
            edad--;
        }
        if (edad < 18)
        {
            throw new InvalidOperationException("El usuario debe ser mayor de 18 años para crear una cuenta.");
        }

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

        var usuario = new Usuario
        {
            Identificacion = dto.Identificacion,
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            Correo = dto.Correo,
            Telefono = dto.Telefono,
            Contrasena = contrasenaHash,
            FechaNacimiento = dto.FechaNacimiento,
            TipoUsuario = "Persona Natural"
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
        return cuenta;
    }

    public Cuenta ObtenerCuenta(string telefono)
    {
        var cuenta = _cuentaRepo.GetByTelefono(telefono);
        if (cuenta == null)
            throw new KeyNotFoundException("Cuenta no encontrada");
        return cuenta;
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
    public List<Cuenta> ObtenerCuentas()
    {
        return _cuentaRepo.GetAll();
    }
}

