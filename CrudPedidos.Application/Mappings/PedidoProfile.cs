using AutoMapper;
using CrudPedidos.Application.DTOs;
using CrudPedidos.Domain.Entities;

namespace CrudPedidos.Application.Mappings;

public class PedidoProfile : Profile
{
    public PedidoProfile()
    {
        CreateMap<Pedido, PedidoDTO>()
            .ForMember(dest => dest.ItensPedido, opt => opt.MapFrom(src => src.ItensPedidoList));

        CreateMap<ItemPedido, ItemPedidoDTO>();

        CreateMap<CriarPedidoDTO, Pedido>()
            .ConstructUsing(src => new Pedido(
                src.NomeCliente,
                src.EmailCliente,
                src.ItensPedido.Select(i => new ItemPedido(
                    i.IdProduto,
                    i.NomeProduto,
                    i.ValorUnitario,
                    i.Quantidade
                )).ToList()
            ))
            .ForMember(dest => dest.ItensPedido, opt => opt.Ignore());

        CreateMap<CriarItemPedidoDTO, ItemPedido>()
            .ConstructUsing(src => new ItemPedido(
                src.IdProduto,
                src.NomeProduto,
                src.ValorUnitario,
                src.Quantidade
            ));
    }
}
