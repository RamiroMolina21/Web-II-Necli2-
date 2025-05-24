using NecliGestion.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Interfaces;

public interface ITokenService {
    string GenerarToken(Usuario usuario);
}
