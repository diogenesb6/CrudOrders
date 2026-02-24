using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CrudOrders.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CrudOrdersContext>
{
    public CrudOrdersContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CrudOrdersContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CrudOrdersDb;Trusted_Connection=true;");

        return new CrudOrdersContext(optionsBuilder.Options);
    }
}
