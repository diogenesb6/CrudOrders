namespace CrudPedidos.Application.DTOs;

public class CreateOrderDTO
{
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public bool Paid { get; set; }
    public List<CreateOrderItemDTO> OrderItems { get; set; } = new();
}
