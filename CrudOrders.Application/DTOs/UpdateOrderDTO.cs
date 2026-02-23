namespace CrudOrders.Application.DTOs;

public class UpdateOrderDTO
{
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public bool Paid { get; set; }
    public List<CreateOrderItemDTO> OrderItems { get; set; } = new();
}
