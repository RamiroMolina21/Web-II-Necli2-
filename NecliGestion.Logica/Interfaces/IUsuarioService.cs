using NecliGestion.Entidades.Entidades;
using NecliGestion.Logica.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Interfaces;

public interface IUsuarioService
{
    ObtenerUsuarioDto ObtenerUsuario(string identificacion);
    void ActualizarUsuario(string identificacion, ActualizarUsuarioDto dto);
    List<ObtenerUsuarioDto> ObtenerUsuarios();
    Usuario Autenticar(string identificacion, string contrasena);

}
