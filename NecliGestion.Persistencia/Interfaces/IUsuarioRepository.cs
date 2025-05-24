using NecliGestion.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Persistencia.Interfaces;

public interface IUsuarioRepository
{
    Usuario GetByIdentificacion(string identificacion);
    Usuario Create(Usuario usuario);
    bool Actualizar(Usuario usuario);
    List<Usuario> GetAll();
    void SaveChanges();
}

