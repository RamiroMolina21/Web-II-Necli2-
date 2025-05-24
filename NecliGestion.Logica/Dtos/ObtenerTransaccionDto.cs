using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Dtos
{
    public record ObtenerTransaccionDto(string NumeroCuentaOrigen, string NumeroCuentaDestino, DateTime FechaTransaccion, decimal Monto, string Tipo);

}
