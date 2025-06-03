using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Interfaces
{
    public interface ICorreoService
    {
        void EnviarCorreo(string destinatario, string asunto, string contenidoHtml);
    }
}
