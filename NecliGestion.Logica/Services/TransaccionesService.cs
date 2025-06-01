using NecliGestion.Entidades.Entidades;
using NecliGestion.Logica.Dtos;
using NecliGestion.Logica.Interfaces;
using NecliGestion.Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using NecliGestion.Logica.Exceptions;

namespace NecliGestion.Logica.Services;

public class TransaccionService : ITransaccionService
{
    private readonly ICuentaRepository _cuentaRepo;
    private readonly ITransaccionRepository _transaccionRepo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private const int TIEMPO_DIFERIDO_HORAS = 2; // Tiempo fijo de 2 horas para todas las transacciones

    public TransaccionService(ICuentaRepository cuentaRepo, ITransaccionRepository transaccionRepo, HttpClient httpClient, IConfiguration config)
    {
        _cuentaRepo = cuentaRepo;
        _transaccionRepo = transaccionRepo;
        _httpClient = httpClient;
        _config = config;
    }

    public TransaccionResultadoDto RealizarTransaccion(string usuarioId, TransaccionDto dto)
    {
        if (dto.Monto < 1000 || dto.Monto > 5000000)
            throw new ArgumentException("El monto debe estar entre $1,000 y $5,000,000");

        var origen = _cuentaRepo.GetByUsuarioId(usuarioId);
        var destino = _cuentaRepo.GetByTelefono(dto.CuentaDestino);

        if (origen == null || destino == null)
            throw new ArgumentException("Cuenta origen o destino no encontrada");

        if (origen.Saldo < dto.Monto)
            throw new InvalidOperationException("Saldo insuficiente");

        origen.Saldo -= dto.Monto;
        destino.Saldo += dto.Monto;

        _cuentaRepo.SaveChanges();

        var transaccion = new Transaccion
        {
            NumeroCuentaOrigen = origen.Telefono,
            NumeroCuentaDestino = dto.CuentaDestino,
            FechaTransaccion = DateTime.Now,
            Monto = dto.Monto,
            Tipo = dto.Tipo
        };

        _transaccionRepo.Create(transaccion);

        return new TransaccionResultadoDto(
            transaccion.NumeroCuentaOrigen,
            transaccion.NumeroCuentaDestino,
            transaccion.Monto,
            transaccion.Tipo,
            transaccion.FechaTransaccion
        );
    }

    public List<ObtenerTransaccionDto> ConsultarTransacciones(string telefono, DateTime? desde, DateTime? hasta)
    {
        var transacciones = _transaccionRepo.GetByCuentaAndFechas(telefono, desde, hasta);

        return transacciones.Select(t => new ObtenerTransaccionDto(
            NumeroCuentaOrigen: t.NumeroCuentaOrigen,
            NumeroCuentaDestino: t.NumeroCuentaDestino,
            FechaTransaccion: t.FechaTransaccion,
            Monto: t.Monto,
            Tipo: t.Tipo
        )).ToList();
    }

    public async Task<bool> ValidarCuentaDestino(string numeroCuenta, string documento, int banco)
    {
        var apiUrl = _config["InterbankApi:ValidationEndpoint"];
        var url = $"{apiUrl}?numeroCuenta={numeroCuenta}&documentoUsuario={documento}&banco={banco}";
        var response = await _httpClient.GetAsync(url);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RealizarTransaccionInterbancaria(string usuarioId, TransaccionInterbancariaDto dto)
    {
        try
        {
            var cuentaOrigen = _cuentaRepo.GetByUsuarioId(usuarioId);
            if (cuentaOrigen == null)
                throw new NegocioException("Cuenta origen no encontrada.");

            if (cuentaOrigen.Telefono == dto.NumeroCuentaDestino.ToString())
                throw new NegocioException("No puedes transferir a tu propia cuenta.");

            var montoMinimo = _config.GetValue<decimal>("TransactionLimits:MinAmount", 1000);
            var montoMaximo = _config.GetValue<decimal>("TransactionLimits:MaxAmount", 5000000);
            var diferidoHoras = _config.GetValue<int>("TransactionLimits:DeferredHours", 2);

            if (dto.Monto < montoMinimo || dto.Monto > montoMaximo)
                throw new NegocioException($"El monto está fuera de los límites permitidos. Mínimo: {montoMinimo}, Máximo: {montoMaximo}");

            if (cuentaOrigen.Saldo < dto.Monto)
                throw new NegocioException("Saldo insuficiente.");

            if (string.IsNullOrEmpty(dto.NumeroDocumento) || dto.NumeroDocumento.Length < 8)
                throw new NegocioException("Número de documento inválido.");

            var cuentaValida = await ValidarCuentaDestino(dto.NumeroCuentaDestino.ToString(), dto.NumeroDocumento, dto.BancoDestino);
            if (!cuentaValida)
                throw new NegocioException("Cuenta destino inválida o no verificada por el banco destino.");

            cuentaOrigen.Saldo -= dto.Monto;
            _cuentaRepo.SaveChanges();

            var fechaTransaccion = DateTime.Now.AddHours(diferidoHoras);

            var transaccion = new Transaccion
            {
                NumeroCuentaOrigen = cuentaOrigen.Telefono,
                NumeroCuentaDestino = $"B{dto.BancoDestino}-{dto.NumeroCuentaDestino}",
                Monto = dto.Monto,
                FechaTransaccion = fechaTransaccion,
                Tipo = $"Interbancario {dto.Moneda} B{dto.BancoDestino} {diferidoHoras}h"
            };

            try
            {
                _transaccionRepo.Create(transaccion);
                return true;
            }
            catch (Exception ex)
            {
                cuentaOrigen.Saldo += dto.Monto;
                _cuentaRepo.SaveChanges();
                throw new NegocioException("Error al registrar la transacción. Se ha revertido el cambio en el saldo.", ex);
            }
        }
        catch (Exception ex)
        {
            throw new NegocioException($"Error al realizar la transacción interbancaria: {ex.Message}", ex);
        }
    }
}
