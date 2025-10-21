using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using Service.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class GenaricRepository<TEntity, TKey>(StoreDBContext _dBContext) : IGenaricRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public async Task AddAsync(TEntity entity)
        {
            await _dBContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task<int> CountAsync(ISpecifications<TEntity, TKey> specifications)
        {
            return await SpecificationEvaluator.
                CreateQuery(_dBContext.Set<TEntity>(), specifications).CountAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dBContext.Set<TEntity>().ToListAsync();   
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> specifications)
        {
            return await SpecificationEvaluator.
                CreateQuery(_dBContext.Set<TEntity>(), specifications).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _dBContext.Set<TEntity>().FindAsync(id); 
        }

        public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, TKey> specifications)
        {
            return await SpecificationEvaluator.
                CreateQuery(_dBContext.Set<TEntity>(), specifications).FirstOrDefaultAsync();
        }

        public void Remove(TEntity entity)
        {
            _dBContext.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _dBContext.Set<TEntity>().Update(entity);
        }
    }
}
