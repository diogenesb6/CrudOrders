using System.ComponentModel.DataAnnotations.Schema;
using CrudOrders.Domain.Resources;

namespace CrudOrders.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public bool Paid { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    private List<OrderItem> _orderItems = new();

    [NotMapped]
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public List<OrderItem> OrderItemsList
    {
        get => _orderItems;
        set => _orderItems = value ?? new();
    }

    public Order() { }

    public Order(string customerName, string customerEmail, List<OrderItem> orderItems)
    {
        ValidateOrder(customerName, customerEmail, orderItems);

        CustomerName = customerName;
        CustomerEmail = customerEmail;
        _orderItems = orderItems;
        CalculateTotalAmount();
    }

    public void AddItem(OrderItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        item.OrderId = Id;
        _orderItems.Add(item);
        CalculateTotalAmount();
    }

    public void RemoveItem(int itemId)
    {
        var item = _orderItems.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            _orderItems.Remove(item);
            CalculateTotalAmount();
        }
    }

    public void UpdateOrder(string customerName, string customerEmail, bool paid)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException(Messages.ExcMSG1, nameof(customerName));

        if (string.IsNullOrWhiteSpace(customerEmail))
            throw new ArgumentException(Messages.ExcMSG2, nameof(customerEmail));

        CustomerName = customerName;
        CustomerEmail = customerEmail;
        Paid = paid;
        UpdatedAt = DateTime.UtcNow;
    }

    public void CalculateTotalAmount()
    {
        TotalAmount = _orderItems.Sum(item => item.CalculateSubtotal());
    }

    private static void ValidateOrder(string customerName, string customerEmail, List<OrderItem> orderItems)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException(Messages.ExcMSG1, nameof(customerName));

        if (string.IsNullOrWhiteSpace(customerEmail))
            throw new ArgumentException(Messages.ExcMSG2, nameof(customerEmail));

        if (orderItems == null || orderItems.Count == 0)
            throw new ArgumentException(Messages.ExcMSG3, nameof(orderItems));
    }
}
