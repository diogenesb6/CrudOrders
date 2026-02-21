namespace CrudPedidos.Application.DTOs;

public class AtualizarPedidoDTO
{
    public string NomeCliente { get; set; } = string.Empty;
    public string EmailCliente { get; set; } = string.Empty;
    public bool Pago { get; set; }
    public List<CriarItemPedidoDTO> ItensPedido { get; set; } = new();
}
