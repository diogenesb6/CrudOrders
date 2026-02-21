using CrudPedidos.Domain.Entities;

namespace CrudPedidos.Domain.Interfaces;

public interface IPedidoRepository
{
    Task<Pedido?> ObterPorIdAsync(int id);
    Task<IEnumerable<Pedido>> ObterTodosAsync();
    Task<Pedido> CriarAsync(Pedido pedido);
    Task<Pedido> AtualizarAsync(Pedido pedido);
    Task<bool> DeletarAsync(int id);
    Task<bool> ExisteAsync(int id);
}
