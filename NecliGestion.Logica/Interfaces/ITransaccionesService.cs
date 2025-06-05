using NecliGestion.Entidades.Entidades;
using NecliGestion.Logica.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Interfaces;

public interface ITransaccionService
{
    TransaccionResultadoDto RealizarTransaccion(string usuarioId, TransaccionDto dto);
    List<ObtenerTransaccionDto> ConsultarTransacciones(string telefono, DateTime? desde, DateTime? hasta);
    Task<bool> ValidarCuentaDestino(string numeroCuenta, string documento, int banco);
    Task<bool> RealizarTransaccionInterbancaria(string usuarioId, TransaccionInterbancariaDto dto);
    Task<List<TransaccionInterbancariaConsultaDto>> ConsultarTransaccionesInterbancariasExternas(string numeroCuenta, DateTime fecha);
}
