using Microsoft.EntityFrameworkCore;
using PedidosApi.Data;
using PedidosApi.Mappings;
using PedidosApi.Services.Implementations;
using PedidosApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//Área de servicios

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

builder.Services.AddSwaggerGen();



builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();

var app = builder.Build();

//Área de middlewares

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();


app.Run();
