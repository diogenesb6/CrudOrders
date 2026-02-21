using CrudPedidos.Application.DTOs;

namespace CrudPedidos.Application.Interfaces;

public interface IPedidoService
{
    Task<PedidoDTO?> ObterPorIdAsync(int id);
    Task<IEnumerable<PedidoDTO>> ObterTodosAsync();
    Task<PedidoDTO> CriarAsync(CriarPedidoDTO dto);
    Task<PedidoDTO> AtualizarAsync(int id, AtualizarPedidoDTO dto);
    Task<bool> DeletarAsync(int id);
}
