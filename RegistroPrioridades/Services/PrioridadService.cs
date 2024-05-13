using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using RegistroPrioridades.DAL;
using RegistroPrioridades.Models;

namespace RegistroPrioridades.Services;

public class PrioridadService
{

    private readonly Contexto _context;
    public PrioridadService(Contexto contexto)
    {
        _context = contexto;
    }

    public async Task<bool> Guardar(Prioridades Prioridad)
    {
        //Busca la prioridad, si no existe la inserta, si existe la modifica
        if (!await Existe(Prioridad.PrioridadId))
            return await Insertar(Prioridad);
        else
            return await Modificar(Prioridad);
    }

    public async Task<bool> Insertar(Prioridades Prioridades)
    {
        _context.Prioridades.Add(Prioridades);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Prioridades Prioridades)
    {
        _context.Update(Prioridades);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Existe(int PrioridadId)
    {
        return await _context.Prioridades
            .AnyAsync(p => p.PrioridadId == PrioridadId);

    }

    public async Task<bool> Existe(string? descripcion, int? prioridadId = null)
    {
        return await _context.Prioridades
            .AnyAsync(p => p.Descripcion.Equals(descripcion));
    }


    public async Task<bool> Existe(int prioridadId, string? descripcion)
    {
        //TODO: Unir los dos existe en uno solo para reducir duplicidad de codigo.
        return await _context.Prioridades
            .AnyAsync(p => p.PrioridadId != prioridadId && p.Descripcion.Equals(descripcion));
    }

    public async Task<bool> Eliminar(int id)
    {
        var prioridades = await _context.Prioridades
            .Where(p => p.PrioridadId == id)
            .ExecuteDeleteAsync();
        return prioridades > 0;
    }

    public async Task<Prioridades?> Buscar(int id)
    {
        return await _context.Prioridades
            .AsNoTracking()
            .FirstOrDefaultAsync(P => P.PrioridadId == id);
    }

    public async Task<List<Prioridades>> Listar(Expression<Func<Prioridades, bool>> criterio)
    {
        return await _context.Prioridades
            .AsNoTracking()
            .Where(criterio)
            .ToListAsync();

    }
}

