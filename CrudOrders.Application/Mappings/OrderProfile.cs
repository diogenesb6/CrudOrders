using AutoMapper;
using CrudOrders.Application.DTOs;
using CrudOrders.Domain.Entities;

namespace CrudOrders.Application.Mappings;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItemsList));

        CreateMap<OrderItem, OrderItemDTO>();

        CreateMap<CreateOrderDTO, Order>()
            .ConstructUsing(src => new Order(
                src.CustomerName,
                src.CustomerEmail,
                src.OrderItems.Select(i => new OrderItem(
                    i.ProductId,
                    i.ProductName,
                    i.UnitPrice,
                    i.Quantity
                )).ToList()
            ))
            .ForMember(dest => dest.OrderItems, opt => opt.Ignore());

        CreateMap<CreateOrderItemDTO, OrderItem>()
            .ConstructUsing(src => new OrderItem(
                src.ProductId,
                src.ProductName,
                src.UnitPrice,
                src.Quantity
            ));
    }
}
