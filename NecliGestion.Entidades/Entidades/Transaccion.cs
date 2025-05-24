using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Entidades.Entidades;

public class Transaccion {
    [Key]
    public int IdTransaccion { get; set; }

    public DateTime FechaTransaccion { get; set; }

    [Required]
    public string NumeroCuentaOrigen { get; set; }

    [Required]
    public string NumeroCuentaDestino { get; set; }

    [Required]
    public decimal Monto { get; set; }

    [Required]
    public string Tipo { get; set; }

    [ForeignKey(nameof(NumeroCuentaOrigen))]
    public Cuenta CuentaOrigen { get; set; }

    [ForeignKey(nameof(NumeroCuentaDestino))]
    public Cuenta CuentaDestino { get; set; }

}
