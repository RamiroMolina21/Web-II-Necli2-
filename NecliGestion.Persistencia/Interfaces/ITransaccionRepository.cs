using NecliGestion.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Persistencia.Interfaces;

public interface ITransaccionRepository
{
    Transaccion Create(Transaccion transaccion);
    List<Transaccion> GetByCuentaAndFechas(string telefono, DateTime? desde, DateTime? hasta);
    void SaveChanges();
}
