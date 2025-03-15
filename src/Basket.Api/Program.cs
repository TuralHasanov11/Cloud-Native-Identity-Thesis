using Basket.Api.Extensions;
using Basket.Api.Features.Basket;

var builder = WebApplication.CreateBuilder(args);

builder.AddBasicServiceDefaults();
builder.AddApplicationServices();

builder.Services.AddGrpc();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapDefaultEndpoints();

app.MapGrpcService<BasketService>();

app.UseDefaultOpenApi();

app.Run();
