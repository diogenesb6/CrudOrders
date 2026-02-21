namespace CrudPedidos.Application.DTOs;

public class CriarItemPedidoDTO
{
    public int IdProduto { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public decimal ValorUnitario { get; set; }
    public int Quantidade { get; set; }
}
