namespace CrudPedidos.Application.DTOs;

public class PedidoDTO
{
    public int Id { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public string EmailCliente { get; set; } = string.Empty;
    public bool Pago { get; set; }
    public decimal ValorTotal { get; set; }
    public List<ItemPedidoDTO> ItensPedido { get; set; } = new();
}
