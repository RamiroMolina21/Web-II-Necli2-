using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Dtos
{
    public record ObtenerUsuarioDto(string Identificacion, string Nombres, string Apellidos, string Correo, string Telefono);

}
