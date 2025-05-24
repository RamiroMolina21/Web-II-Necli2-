using NecliGestion.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Persistencia.Interfaces;

public interface ICuentaRepository
{
    Cuenta GetByTelefono(string telefono);
    Cuenta Create(Cuenta cuenta);
    void Delete(Cuenta cuenta);
    List<Cuenta> GetAll();
    void SaveChanges();
}
