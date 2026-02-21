using CrudPedidos.Application.Mappings;
using CrudPedidos.Infrastructure.Data;
using CrudPedidos.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Entity Framework
builder.Services.AddDbContext<CrudPedidosContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.UseInMemoryDatabase("CrudPedidosDb");
    }
    else
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        options.UseSqlServer(connectionString, x => x.MigrationsAssembly("CrudPedidos.Infrastructure"));
    }
});

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<PedidoProfile>();
});

// Application Services
builder.Services.AddInfrastructureServices();

// Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "CrudPedidos API",
        Version = "v1",
        Description = "API para gerenciamento de pedidos com CRUD completo",
        Contact = new()
        {
            Name = "Diogenes",
            Url = new Uri("https://github.com/diogenesb6")
        },
        License = new()
        {
            Name = "MIT",
            Url = new Uri("https://github.com/diogenesb6/CrudPedidos/blob/main/LICENSE")
        }
    });
});

var app = builder.Build();

// Migrate database
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<CrudPedidosContext>();
        if (db.Database.IsRelational())
        {
            await db.Database.MigrateAsync();
        }
        else
        {
            await db.Database.EnsureCreatedAsync();
        }
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CrudPedidos API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();
