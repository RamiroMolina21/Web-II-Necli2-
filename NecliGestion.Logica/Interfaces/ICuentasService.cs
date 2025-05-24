using NecliGestion.Entidades.Entidades;
using NecliGestion.Logica.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Interfaces;

public interface ICuentaService
{
    Cuenta CrearCuenta(CrearCuentaDto dto);

    ObtenerCuentaDto ObtenerCuenta(string telefono);
    void EliminarCuenta(string telefono);

    List<ObtenerCuentaDto> ObtenerCuentas();
}
