using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Dtos
{
    public record ActualizarUsuarioDto(
     string Nombres,
     string Apellidos,
     string Correo,
     string? Contrasena,
     string? ContrasenaAnterior
 );
}
