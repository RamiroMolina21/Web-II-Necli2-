using System;

namespace NecliGestion.Logica.Dtos
{
    public record TransaccionInterbancariaDto(
     long NumeroCuentaDestino,
     string NumeroDocumento,
     int BancoDestino,
     decimal Monto,
     string Moneda = "COP"
 );

}