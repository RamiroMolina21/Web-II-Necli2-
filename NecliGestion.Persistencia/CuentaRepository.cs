using Microsoft.EntityFrameworkCore;
using NecliGestion.Entidades.Entidades;
using NecliGestion.Persistencia.DbContexts;
using NecliGestion.Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Persistencia;

public class CuentaRepository : ICuentaRepository
{
    private readonly NecliDbContext _context;

    public CuentaRepository(NecliDbContext context)
    {
        _context = context;
    }

    public Cuenta GetByTelefono(string telefono)
    {
        return _context.Cuentas.Include(c => c.Usuario).FirstOrDefault(c => c.Telefono == telefono);
    }

    public Cuenta Create(Cuenta cuenta)
    {
        _context.Cuentas.Add(cuenta);
        SaveChanges();
        return cuenta;
    }

    public void Delete(Cuenta cuenta)
    {
        _context.Cuentas.Remove(cuenta);
        SaveChanges();
    }

    public List<Cuenta> GetAll()
    {
        return _context.Cuentas.Include(c => c.Usuario).ToList();
    }

    public Cuenta GetByUsuarioId(string usuarioId)
    {
        return _context.Cuentas
            .Include(c => c.Usuario)
            .FirstOrDefault(c => c.Usuario.Identificacion == usuarioId);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
