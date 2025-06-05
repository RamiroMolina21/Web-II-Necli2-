using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Dtos
{
    public record TransaccionInterbancariaConsultaDto(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("numero_cuenta")] string NumeroCuenta,
        [property: JsonPropertyName("banco")] string Banco,
        [property: JsonPropertyName("monto")] decimal Monto,
        [property: JsonPropertyName("moneda")] string Moneda,
        [property: JsonPropertyName("fecha")] string Fecha, 
        [property: JsonPropertyName("numero_cuenta_destino")] string NumeroCuentaDestino
    );
}
