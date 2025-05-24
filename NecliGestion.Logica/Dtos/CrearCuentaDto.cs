using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Dtos
{
    public record CrearCuentaDto(
      string Identificacion,
      string Nombres,
      string Apellidos,
      string Correo,
      string Telefono,
      string Contrasena,
      DateTime FechaNacimiento
  );
}
