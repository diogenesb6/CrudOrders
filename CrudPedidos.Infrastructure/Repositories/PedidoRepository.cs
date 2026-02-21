using CrudPedidos.Domain.Entities;
using CrudPedidos.Domain.Interfaces;
using CrudPedidos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CrudPedidos.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly CrudPedidosContext _context;

    public PedidoRepository(CrudPedidosContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Pedido?> ObterPorIdAsync(int id)
    {
        return await _context.Pedidos
            .Include(p => p.ItensPedido)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Pedido>> ObterTodosAsync()
    {
        return await _context.Pedidos
            .Include(p => p.ItensPedido)
            .OrderByDescending(p => p.DataCriacao)
            .ToListAsync();
    }

    public async Task<Pedido> CriarAsync(Pedido pedido)
    {
        if (pedido == null)
            throw new ArgumentNullException(nameof(pedido));

        await _context.Pedidos.AddAsync(pedido);
        await _context.SaveChangesAsync();

        return pedido;
    }

    public async Task<Pedido> AtualizarAsync(Pedido pedido)
    {
        if (pedido == null)
            throw new ArgumentNullException(nameof(pedido));

        _context.Pedidos.Update(pedido);
        await _context.SaveChangesAsync();

        return pedido;
    }

    public async Task<bool> DeletarAsync(int id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido == null)
            return false;

        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ExisteAsync(int id)
    {
        return await _context.Pedidos.AnyAsync(p => p.Id == id);
    }
}
