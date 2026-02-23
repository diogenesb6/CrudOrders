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
            entity.ToTable("Pedidos");

            entity.Property(e => e.CustomerName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("NomeCliente");

            entity.Property(e => e.CustomerEmail)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("EmailCliente");

            entity.Property(e => e.Paid)
                .HasDefaultValue(false)
                .HasColumnName("Pago");

            entity.Property(e => e.TotalAmount)
                .HasPrecision(18, 2)
                .HasColumnName("ValorTotal");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnName("DataCriacao");

            entity.Property(e => e.UpdatedAt)
                .HasColumnName("DataAtualizacao");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("ItensPedido");

            entity.Property(e => e.ProductId)
                .HasColumnName("IdProduto");

            entity.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("NomeProduto");

            entity.Property(e => e.UnitPrice)
                .HasPrecision(18, 2)
                .HasColumnName("ValorUnitario");

            entity.Property(e => e.Quantity)
                .IsRequired()
                .HasColumnName("Quantidade");

            entity.Property(e => e.OrderId)
                .IsRequired()
                .HasColumnName("PedidoId");

            entity.HasOne(i => i.Order)
                .WithMany(p => p.OrderItemsList)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });
    }
}
