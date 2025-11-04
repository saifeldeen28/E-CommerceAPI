using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistance.DataSeed
{
    public class DataSeeding(StoreDBContext _dBContext,UserManager<ApplicationUser> _userManager,RoleManager<IdentityRole> _roleManager) : IDataSeeding
    {
        public async Task DataSeedAsync()
        {
            var Pending = await _dBContext.Database.GetPendingMigrationsAsync();
            if (Pending.Any()) 
            {
                _dBContext.Database.Migrate();
            }

            if (!_dBContext.ProductBrands.Any())
            { 
                var BrandsData = File.OpenRead(@"../Infrastructure/Persistance/DataSeed/brands.json");
                var Brands =await  JsonSerializer.DeserializeAsync<List<ProductBrand>>(BrandsData);
                if(Brands is not null && Brands.Any())   
                {
                    foreach (var item in Brands)
                    {
                        _dBContext.ProductBrands.Add(item);
                    }
                }
                var TypesData = File.OpenRead(@"../Infrastructure/Persistance/DataSeed/types.json");
                var Types =await JsonSerializer.DeserializeAsync<List<ProductType>>(TypesData);
                if (Types is not null && Types.Any())
                {
                    foreach (var item in Types)
                    {
                        _dBContext.ProductTypes.Add(item);
                    }
                }
                var ProductsData = File.OpenRead(@"../Infrastructure/Persistance/DataSeed/products.json");
                var Products = await JsonSerializer.DeserializeAsync<List<Product>>(ProductsData);
                if (Products is not null && Products.Any())
                {
                    foreach (var item in Products)
                    {
                        _dBContext.Products.Add(item);
                    }
                }
                await _dBContext.SaveChangesAsync();
            }
            if (!_dBContext.DeliveryMethods.Any())
            {
                var ProductsData = File.OpenRead(@"../Infrastructure/Persistance/DataSeed/delivery.json");
                var Products = await JsonSerializer.DeserializeAsync<List<DeliveryMethod>>(ProductsData);
                if (Products is not null && Products.Any())
                {
                    foreach (var item in Products)
                    {
                        _dBContext.DeliveryMethods.Add(item);
                    }
                }

                await _dBContext.SaveChangesAsync();
            }
        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    var roles = new List<IdentityRole>
                {
                    new IdentityRole{Name="SuperAdmin"},
                    new IdentityRole{Name="Admin"},
                };
                    foreach (var role in roles)
                    {
                        _roleManager.CreateAsync(role).Wait();
                    }
                }
                if (!_userManager.Users.Any())
                {
                    var user01 = new ApplicationUser
                    {
                        DisplayName = "saif eldeen",
                        Email = "saif@gmail.com",
                        PhoneNumber = "1234567890",
                        UserName = "Saifeldeen",
                        EmailConfirmed = true,
                    };
                    var user02 = new ApplicationUser
                    {
                        DisplayName = "joe",
                        Email = "joe@gmail.com",
                        PhoneNumber = "1234567890",
                        UserName = "joeseph",
                        EmailConfirmed = true,
                    };
                    await _userManager.CreateAsync(user01, "Pa$$w0rd");
                    await _userManager.CreateAsync(user02, "Pa$$w0rd");
                    await _userManager.AddToRoleAsync(user01, "Admin");
                    await _userManager.AddToRoleAsync(user02, "SuperAdmin");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        
    }
}
