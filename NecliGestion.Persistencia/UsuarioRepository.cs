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

public class UsuarioRepository : IUsuarioRepository
{
    private readonly NecliDbContext _context;

    public UsuarioRepository(NecliDbContext context)
    {
        _context = context;
    }

    public Usuario GetByIdentificacion(string identificacion)
    {
        return _context.Usuarios.Include(u => u.Cuenta).FirstOrDefault(u => u.Identificacion == identificacion);
    }

    public Usuario Create(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        SaveChanges();  
        return usuario;
    }


    public bool Actualizar(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        return _context.SaveChanges()>0;
    }

    public List<Usuario> GetAll()
    {
        return _context.Usuarios.Include(u => u.Cuenta).ToList();
    }

    public Usuario GetByCorreo(string correo)
    {
        return _context.Usuarios.Include(u => u.Cuenta)
        .FirstOrDefault(u => u.Correo == correo);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
