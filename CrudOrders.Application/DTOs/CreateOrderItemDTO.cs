using System.Text.Json.Serialization;

namespace CrudOrders.Application.DTOs;

public class CreateOrderItemDTO
{
    [JsonPropertyName("idProduto")]
    public int ProductId { get; set; }

    [JsonPropertyName("nomeProduto")]
    public string ProductName { get; set; } = string.Empty;

    [JsonPropertyName("valorUnitario")]
    public decimal UnitPrice { get; set; }

    [JsonPropertyName("quantidade")]
    public int Quantity { get; set; }
}
