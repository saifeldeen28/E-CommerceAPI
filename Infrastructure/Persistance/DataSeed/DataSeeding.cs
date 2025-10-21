using DomainLayer.Contracts;
using DomainLayer.Models;
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
    public class DataSeeding(StoreDBContext _dBContext) : IDataSeeding
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
        }
    }
}
