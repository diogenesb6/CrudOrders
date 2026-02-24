using CrudOrders.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrudOrders.Infrastructure.Data;

public class CrudOrdersContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    public CrudOrdersContext(DbContextOptions<CrudOrdersContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Orders");

            entity.Property(e => e.CustomerName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.CustomerEmail)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Paid)
                .HasDefaultValue(false);

            entity.Property(e => e.TotalAmount)
                .HasPrecision(18, 2);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.UpdatedAt);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("OrderItems");

            entity.Property(e => e.ProductId);

            entity.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.UnitPrice)
                .HasPrecision(18, 2);

            entity.Property(e => e.Quantity)
                .IsRequired();

            entity.Property(e => e.OrderId)
                .IsRequired();

            entity.HasOne(i => i.Order)
                .WithMany(p => p.OrderItemsList)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });
    }
}
