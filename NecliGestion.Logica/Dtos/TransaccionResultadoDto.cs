using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Dtos
{
    public record TransaccionResultadoDto(
        string NumeroCuentaOrigen,
        string NumeroCuentaDestino,
        decimal Monto,
        string Tipo,
        DateTime FechaTransaccion
    );
}
