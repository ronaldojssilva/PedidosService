using PedidosService.API.Extensions;
using PedidosService.API.Middlewares;
using PedidosService.Application.Http;
using PedidosService.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddPedidosModule(builder.Configuration);

builder.Services.AddHttpClient<IHttpClientService, HttpClientService>(client =>
{
    var url = builder.Configuration["ExternalServices:SistemaB"];
    client.BaseAddress = new Uri(url!);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Pedidos Service API",
        Version = "v1",
        Description = "API para gerenciamento de pedidos",
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();

public partial class Program;