using AutoMapper;
using CrudPedidos.Application.DTOs;
using CrudPedidos.Application.Interfaces;
using CrudPedidos.Domain.Entities;
using CrudPedidos.Domain.Interfaces;

namespace CrudPedidos.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _repository;
    private readonly IMapper _mapper;

    public PedidoService(IPedidoRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PedidoDTO?> ObterPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Id deve ser maior que zero", nameof(id));

        var pedido = await _repository.ObterPorIdAsync(id);
        return pedido == null ? null : _mapper.Map<PedidoDTO>(pedido);
    }

    public async Task<IEnumerable<PedidoDTO>> ObterTodosAsync()
    {
        var pedidos = await _repository.ObterTodosAsync();
        return _mapper.Map<IEnumerable<PedidoDTO>>(pedidos);
    }

    public async Task<PedidoDTO> CriarAsync(CriarPedidoDTO dto)
    {
        ValidarCriarPedidoDTO(dto);

        var pedido = _mapper.Map<Pedido>(dto);
        var pedidoCriado = await _repository.CriarAsync(pedido);

        return _mapper.Map<PedidoDTO>(pedidoCriado);
    }

    public async Task<PedidoDTO> AtualizarAsync(int id, AtualizarPedidoDTO dto)
    {
        if (id <= 0)
            throw new ArgumentException("Id deve ser maior que zero", nameof(id));

        ValidarAtualizarPedidoDTO(dto);

        var pedido = await _repository.ObterPorIdAsync(id);
        if (pedido == null)
            throw new InvalidOperationException($"Pedido com id {id} não encontrado");

        pedido.AtualizarPedido(dto.NomeCliente, dto.EmailCliente, dto.Pago);

        var pedidoAtualizado = await _repository.AtualizarAsync(pedido);
        return _mapper.Map<PedidoDTO>(pedidoAtualizado);
    }

    public async Task<bool> DeletarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Id deve ser maior que zero", nameof(id));

        var existe = await _repository.ExisteAsync(id);
        if (!existe)
            throw new InvalidOperationException($"Pedido com id {id} não encontrado");

        return await _repository.DeletarAsync(id);
    }

    private static void ValidarCriarPedidoDTO(CriarPedidoDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.NomeCliente))
            throw new ArgumentException("Nome do cliente é obrigatório", nameof(dto.NomeCliente));

        if (string.IsNullOrWhiteSpace(dto.EmailCliente))
            throw new ArgumentException("Email do cliente é obrigatório", nameof(dto.EmailCliente));

        if (dto.ItensPedido == null || dto.ItensPedido.Count == 0)
            throw new ArgumentException("Pedido deve conter pelo menos um item", nameof(dto.ItensPedido));

        foreach (var item in dto.ItensPedido)
        {
            if (item.ValorUnitario <= 0)
                throw new ArgumentException("Valor unitário deve ser maior que zero", nameof(item.ValorUnitario));

            if (item.Quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero", nameof(item.Quantidade));
        }
    }

    private static void ValidarAtualizarPedidoDTO(AtualizarPedidoDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.NomeCliente))
            throw new ArgumentException("Nome do cliente é obrigatório", nameof(dto.NomeCliente));

        if (string.IsNullOrWhiteSpace(dto.EmailCliente))
            throw new ArgumentException("Email do cliente é obrigatório", nameof(dto.EmailCliente));
    }
}
