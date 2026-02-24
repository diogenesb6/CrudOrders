using CrudOrders.Domain.Entities;
using CrudOrders.Domain.Interfaces;
using CrudOrders.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CrudOrders.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly CrudOrdersContext _context;

    public OrderRepository(CrudOrdersContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders
            .Include(p => p.OrderItemsList)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(p => p.OrderItemsList)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Order> CreateAsync(Order order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        var existingItems = await _context.OrderItems
            .Where(i => i.OrderId == order.Id)
            .ToListAsync();

        _context.OrderItems.RemoveRange(existingItems);
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return false;

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Orders.AnyAsync(p => p.Id == id);
    }
}
