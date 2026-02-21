using CrudPedidos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrudPedidos.Infrastructure.Data;

public class CrudPedidosContext : DbContext
{
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }

    public CrudPedidosContext(DbContextOptions<CrudPedidosContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.NomeCliente)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.EmailCliente)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Pago)
                .HasDefaultValue(false);

            entity.Property(e => e.ValorTotal)
                .HasPrecision(18, 2);

            entity.Property(e => e.DataCriacao)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasMany<ItemPedido>()
                .WithOne(i => i.Pedido)
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ItemPedido>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.NomeProduto)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.ValorUnitario)
                .HasPrecision(18, 2);

            entity.Property(e => e.Quantidade)
                .IsRequired();

            entity.Property(e => e.PedidoId)
                .IsRequired();
        });
    }
}
