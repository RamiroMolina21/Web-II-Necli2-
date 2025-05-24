using NecliGestion.Entidades.Entidades;
using NecliGestion.Logica.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Interfaces;

public interface ITransaccionService
{
    TransaccionResultadoDto RealizarTransaccion(TransaccionDto dto);
    List<Transaccion> ConsultarTransacciones(string telefono, DateTime? desde, DateTime? hasta);
}
