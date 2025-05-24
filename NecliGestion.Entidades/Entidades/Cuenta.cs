using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Entidades.Entidades;

public class Cuenta {

    [Key]
    public int IdCuenta { get; set; }

    [Required]
    public string Telefono { get; set; }

    [Required]
    public string Nombres { get; set; }

    [Required]
    public decimal Saldo { get; set; }

    public DateTime FechaCreacion { get; set; }

    // Clave externa explícita
    public int UsuarioId { get; set; }

    [ForeignKey(nameof(UsuarioId))]
    public Usuario Usuario { get; set; }

    public ICollection<Transaccion> TransaccionesEnviadas { get; set; }
    public ICollection<Transaccion> TransaccionesRecibidas { get; set; }
}
