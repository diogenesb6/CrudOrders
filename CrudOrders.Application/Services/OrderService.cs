using AutoMapper;
using CrudOrders.Application.DTOs;
using CrudOrders.Application.Interfaces;
using CrudOrders.Application.Resources;
using CrudOrders.Domain.Entities;
using CrudOrders.Domain.Interfaces;

namespace CrudOrders.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<OrderDTO?> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException(Messages.ExcMSG6, nameof(id));

        var order = await _repository.GetByIdAsync(id);
        return order == null ? null : _mapper.Map<OrderDTO>(order);
    }

    public async Task<IEnumerable<OrderDTO>> GetAllAsync()
    {
        var orders = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<OrderDTO>>(orders);
    }

    public async Task<OrderDTO> CreateAsync(CreateOrderDTO dto)
    {
        ValidateCreateOrderDTO(dto);

        var order = _mapper.Map<Order>(dto);
        var createdOrder = await _repository.CreateAsync(order);

        return _mapper.Map<OrderDTO>(createdOrder);
    }

    public async Task<OrderDTO> UpdateAsync(int id, UpdateOrderDTO dto)
    {
        if (id <= 0)
            throw new ArgumentException(Messages.ExcMSG6, nameof(id));

        ValidateUpdateOrderDTO(dto);

        var order = await _repository.GetByIdAsync(id);
        if (order == null)
            throw new InvalidOperationException(string.Format(Messages.ExcMSG7, id));

        order.UpdateOrder(dto.CustomerName, dto.CustomerEmail, dto.Paid);

        var newItems = dto.OrderItems?.Select(i => new OrderItem(
            i.ProductId, i.ProductName, i.UnitPrice, i.Quantity
        )).ToList() ?? new List<OrderItem>();

        order.OrderItemsList.Clear();
        foreach (var item in newItems)
        {
            item.OrderId = order.Id;
            order.OrderItemsList.Add(item);
        }
        order.CalculateTotalAmount();

        var updatedOrder = await _repository.UpdateAsync(order);
        return _mapper.Map<OrderDTO>(updatedOrder);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException(Messages.ExcMSG6, nameof(id));

        var exists = await _repository.ExistsAsync(id);
        if (!exists)
            throw new InvalidOperationException(string.Format(Messages.ExcMSG7, id));

        return await _repository.DeleteAsync(id);
    }

    private static void ValidateCreateOrderDTO(CreateOrderDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.CustomerName))
            throw new ArgumentException(Messages.ExcMSG1, nameof(dto.CustomerName));

        if (string.IsNullOrWhiteSpace(dto.CustomerEmail))
            throw new ArgumentException(Messages.ExcMSG2, nameof(dto.CustomerEmail));

        if (dto.OrderItems == null || dto.OrderItems.Count == 0)
            throw new ArgumentException(Messages.ExcMSG3, nameof(dto.OrderItems));

        foreach (var item in dto.OrderItems)
        {
            if (item.UnitPrice <= 0)
                throw new ArgumentException(Messages.ExcMSG4, nameof(item.UnitPrice));

            if (item.Quantity <= 0)
                throw new ArgumentException(Messages.ExcMSG5, nameof(item.Quantity));
        }
    }

    private static void ValidateUpdateOrderDTO(UpdateOrderDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.CustomerName))
            throw new ArgumentException(Messages.ExcMSG1, nameof(dto.CustomerName));

        if (string.IsNullOrWhiteSpace(dto.CustomerEmail))
            throw new ArgumentException(Messages.ExcMSG2, nameof(dto.CustomerEmail));
    }
}
