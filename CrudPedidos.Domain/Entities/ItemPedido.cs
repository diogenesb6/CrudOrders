namespace CrudPedidos.Domain.Entities;

public class ItemPedido
{
    public int Id { get; set; }
    public int IdProduto { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public decimal ValorUnitario { get; set; }
    public int Quantidade { get; set; }

    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }

    public ItemPedido() { }

    public ItemPedido(int idProduto, string nomeProduto, decimal valorUnitario, int quantidade)
    {
        ValidarItemPedido(valorUnitario, quantidade);
        
        IdProduto = idProduto;
        NomeProduto = nomeProduto;
        ValorUnitario = valorUnitario;
        Quantidade = quantidade;
    }

    public decimal CalcularSubtotal() => ValorUnitario * Quantidade;

    private static void ValidarItemPedido(decimal valorUnitario, int quantidade)
    {
        if (valorUnitario <= 0)
            throw new ArgumentException("Valor unitário deve ser maior que zero", nameof(valorUnitario));

        if (quantidade <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero", nameof(quantidade));
    }
}
