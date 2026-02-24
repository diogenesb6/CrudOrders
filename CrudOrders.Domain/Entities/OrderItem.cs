using CrudOrders.Domain.Resources;

namespace CrudOrders.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }

    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public OrderItem() { }

    public OrderItem(int productId, string productName, decimal unitPrice, int quantity)
    {
        ValidateOrderItem(unitPrice, quantity);

        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public decimal CalculateSubtotal() => UnitPrice * Quantity;

    private static void ValidateOrderItem(decimal unitPrice, int quantity)
    {
        if (unitPrice <= 0)
            throw new ArgumentException(Messages.ExcMSG4, nameof(unitPrice));

        if (quantity <= 0)
            throw new ArgumentException(Messages.ExcMSG5, nameof(quantity));
    }
}
