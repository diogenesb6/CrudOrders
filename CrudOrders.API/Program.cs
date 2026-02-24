using CrudOrders.Application.Mappings;
using CrudOrders.Infrastructure.Data;
using CrudOrders.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(
                  "http://localhost:5173",
                  "http://localhost")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

builder.Services.AddDbContext<CrudOrdersContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.UseInMemoryDatabase("CrudOrdersDb");
    }
    else
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        options.UseSqlServer(connectionString, x => x.MigrationsAssembly("CrudOrders.Infrastructure"));
    }
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<OrderProfile>();
});

builder.Services.AddInfrastructureServices();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "CrudOrders API",
        Version = "v1",
        Description = "Full CRUD API for order management",
        Contact = new()
        {
            Name = "Diogenes",
            Url = new Uri("https://github.com/diogenesb6")
        },
        License = new()
        {
            Name = "MIT",
            Url = new Uri("https://github.com/diogenesb6/CrudOrders/blob/main/LICENSE")
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CrudOrdersContext>();
    if (db.Database.IsRelational())
    {
        await db.Database.MigrateAsync();
    }
    else
    {
        await db.Database.EnsureCreatedAsync();
    }
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CrudOrders API v1");
    c.RoutePrefix = "swagger";
});

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("FrontendPolicy");

app.UseRouting();

app.MapControllers();

app.Run();
