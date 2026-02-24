using System.Text.Json.Serialization;

namespace CrudOrders.Application.DTOs;

public class UpdateOrderDTO
{
    [JsonPropertyName("nomeCliente")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonPropertyName("emailCliente")]
    public string CustomerEmail { get; set; } = string.Empty;

    [JsonPropertyName("pago")]
    public bool Paid { get; set; }

    [JsonPropertyName("itensPedido")]
    public List<CreateOrderItemDTO> OrderItems { get; set; } = new();
}
