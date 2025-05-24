using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Entidades.Entidades;

public class Usuario {

    [Key]
    public int IdUsuario { get; set; }

    [Required]
    public string Identificacion { get; set; }

    [Required]
    public string Telefono { get; set; }

    [Required]
    public string? TipoUsuario{ get; set; }

    [Required]
    public string Nombres { get; set; }

    [Required]
    public string Apellidos { get; set; }

    [Required]
    public string Correo { get; set; }

    [Required]
    public string Contrasena { get; set; }

    [Required]
    public DateTime FechaNacimiento { get; set; }

    public Cuenta Cuenta { get; set; }
}
