using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Dtos
{
    public record ObtenerCuentaDto(
    string Telefono,
    string Nombres,
    string Apellidos,
    string Correo,
    decimal Saldo,
    DateTime FechaCreacion
);
}
