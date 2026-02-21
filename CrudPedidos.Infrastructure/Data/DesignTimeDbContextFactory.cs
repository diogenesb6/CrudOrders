using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CrudPedidos.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CrudPedidosContext>
{
    public CrudPedidosContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CrudPedidosContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CrudPedidosDb;Trusted_Connection=true;");

        return new CrudPedidosContext(optionsBuilder.Options);
    }
}
