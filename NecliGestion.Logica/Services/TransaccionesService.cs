using NecliGestion.Entidades.Entidades;
using NecliGestion.Logica.Dtos;
using NecliGestion.Logica.Interfaces;
using NecliGestion.Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Services;

 public class TransaccionService : ITransaccionService
{
    private readonly ICuentaRepository _cuentaRepo;
    private readonly ITransaccionRepository _transaccionRepo;

    public TransaccionService(ICuentaRepository cuentaRepo, ITransaccionRepository transaccionRepo)
    {
        _cuentaRepo = cuentaRepo;
        _transaccionRepo = transaccionRepo;
    }

    public TransaccionResultadoDto RealizarTransaccion(TransaccionDto dto)
    {
        if (dto.Monto < 1000 || dto.Monto > 5000000)
            throw new ArgumentException("El monto debe estar entre $1,000 y $5,000,000");

        var origen = _cuentaRepo.GetByTelefono(dto.CuentaOrigen);
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
            NumeroCuentaOrigen = dto.CuentaOrigen,
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

}
