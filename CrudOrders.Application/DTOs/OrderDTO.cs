using System.Text.Json.Serialization;

namespace CrudOrders.Application.DTOs;

public class OrderDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nomeCliente")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonPropertyName("emailCliente")]
    public string CustomerEmail { get; set; } = string.Empty;

    [JsonPropertyName("pago")]
    public bool Paid { get; set; }

    [JsonPropertyName("valorTotal")]
    public decimal TotalAmount { get; set; }

    [JsonPropertyName("itensPedido")]
    public List<OrderItemDTO> OrderItems { get; set; } = new();
}
