using CrudOrders.Application.DTOs;

namespace CrudOrders.Application.Interfaces;

public interface IOrderService
{
    Task<OrderDTO?> GetByIdAsync(int id);
    Task<IEnumerable<OrderDTO>> GetAllAsync();
    Task<OrderDTO> CreateAsync(CreateOrderDTO dto);
    Task<OrderDTO> UpdateAsync(int id, UpdateOrderDTO dto);
    Task<bool> DeleteAsync(int id);
}
