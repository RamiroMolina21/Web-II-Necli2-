using Microsoft.EntityFrameworkCore;
using NecliGestion.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Persistencia.DbContexts;

public class NecliDbContext : Microsoft.EntityFrameworkCore.DbContext {

    public Microsoft.EntityFrameworkCore.DbSet<Usuario> Usuarios { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Cuenta> Cuentas { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Transaccion> Transacciones { get; set; }
    public NecliDbContext(DbContextOptions<NecliDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tablas
        modelBuilder.Entity<Usuario>().ToTable("Usuario");
        modelBuilder.Entity<Cuenta>().ToTable("Cuenta");
        modelBuilder.Entity<Transaccion>().ToTable("Transaccion");

        // Telefono como único en Cuenta
        modelBuilder.Entity<Cuenta>()
            .HasIndex(c => c.Telefono)
            .IsUnique();

        // Relación 1 a 1 Usuario -> Cuenta
        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Cuenta)
            .WithOne(c => c.Usuario)
            .HasForeignKey<Cuenta>(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relaciones Transaccion -> Cuenta (por teléfono)
        modelBuilder.Entity<Transaccion>()
            .HasOne(t => t.CuentaOrigen)
            .WithMany(c => c.TransaccionesEnviadas)
            .HasForeignKey(t => t.NumeroCuentaOrigen)
            .HasPrincipalKey(c => c.Telefono)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración para cuenta destino (permite NULL para transacciones interbancarias)
        modelBuilder.Entity<Transaccion>()
            .HasOne(t => t.CuentaDestino)
            .WithMany(c => c.TransaccionesRecibidas)
            .HasForeignKey(t => t.NumeroCuentaDestino)
            .HasPrincipalKey(c => c.Telefono)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);

        // Configuración adicional para Transaccion
        modelBuilder.Entity<Transaccion>()
            .Property(t => t.Monto)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Transaccion>()
            .Property(t => t.Tipo)
            .HasMaxLength(100);
    }
}
