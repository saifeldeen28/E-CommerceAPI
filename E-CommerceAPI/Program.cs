using DomainLayer.Contracts;
using E_CommerceAPI.CustomMiddleWares;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using Persistance.DataSeed;
using Persistance.Repositories;
using Service;
using ServiceAbstraction;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<StoreDBContext>(options=>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IDataSeeding, DataSeeding>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(x=>x.AddProfile(new MappingProfiles()));
builder.Services.AddScoped<IServiceManger, ServiceManger>();
builder.Services.AddTransient<Service.PictureUrlResolver>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    var configuration = builder.Configuration.GetConnectionString("RedisConnection");
    return ConnectionMultiplexer.Connect(configuration);
});

var app = builder.Build();
var scope = app.Services.CreateScope();
var dataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
await dataSeeding.DataSeedAsync();

app.UseMiddleware<CustomExceptionHandlerMiddleWare>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
