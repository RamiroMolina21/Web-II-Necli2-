using NecliGestion.Entidades.Entidades;
using NecliGestion.Persistencia.DbContexts;
using NecliGestion.Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Persistencia;

public class TransaccionRepository : ITransaccionRepository
{
    private readonly NecliDbContext _context;

    public TransaccionRepository(NecliDbContext context)
    {
        _context = context;
    }

    public Transaccion Create(Transaccion transaccion)
    {
        _context.Transacciones.Add(transaccion);
        SaveChanges();
        return transaccion;
    }

    public List<Transaccion> GetByCuentaAndFechas(string telefono, DateTime? desde, DateTime? hasta)
    {
        var query = _context.Transacciones
            .Where(t => t.NumeroCuentaOrigen == telefono || t.NumeroCuentaDestino == telefono);

        if (desde.HasValue)
            query = query.Where(t => t.FechaTransaccion >= desde.Value);

        if (hasta.HasValue)
            query = query.Where(t => t.FechaTransaccion <= hasta.Value);

        return query.OrderByDescending(t => t.FechaTransaccion).ToList();
    }


    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
