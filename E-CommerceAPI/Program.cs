using DomainLayer.Contracts;
using DomainLayer.Models;
using E_CommerceAPI.CustomMiddleWares;
using E_CommerceAPI.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using Persistance.DataSeed;
using Persistance.Repositories;
using Service;
using ServiceAbstraction;
using Shared;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<StoreDBContext>(options=>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDbContext<StoreIdentityDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});
builder.Services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<StoreIdentityDBContext>();
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
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState.Where(e => e.Value.Errors.Count > 0)
           .Select(m=> new ValidationErrors() {
               Errors=m.Value.Errors.Select(e=>e.ErrorMessage),
               Field=m.Key
           });
        var errorResponse = new ValidationErrorToReturn()
        {
            Errors = errors
        };
        return new BadRequestObjectResult(errorResponse);
    };
});
builder.Services.AddJWTServices(builder.Configuration);

var app = builder.Build();
var scope = app.Services.CreateScope();
var dataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
await dataSeeding.DataSeedAsync();
await dataSeeding.IdentityDataSeedAsync();

app.UseMiddleware<CustomExceptionHandlerMiddleWare>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
