using Microsoft.EntityFrameworkCore;
using NecliGestion.Logica.Interfaces;
using NecliGestion.Logica.Services;
using NecliGestion.Persistencia;
using NecliGestion.Persistencia.DbContexts;
using NecliGestion.Persistencia.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ICuentaService, CuentaService>();
builder.Services.AddScoped<ITransaccionService, TransaccionService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ICuentaRepository, CuentaRepository>();
builder.Services.AddScoped<ITransaccionRepository, TransaccionRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();



var conexion = builder.Configuration.GetConnectionString("local");
builder.Services.AddDbContext<NecliDbContext>(option => option.UseSqlServer(conexion));

builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
