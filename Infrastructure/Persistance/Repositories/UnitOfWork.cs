using DomainLayer.Contracts;
using DomainLayer.Models;
using Persistance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class UnitOfWork(StoreDBContext _dBContext) : IUnitOfWork
    {
        private readonly Dictionary<String, object> _repositories = new();
        public IGenaricRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var type = typeof(TEntity).Name;
            if (_repositories.ContainsKey(type))
            {
                return (IGenaricRepository<TEntity, TKey>)_repositories[type];
            }
            var repository = new GenaricRepository<TEntity, TKey>(_dBContext);
            _repositories.Add(type, repository);
            return repository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dBContext.SaveChangesAsync();
        }
    }
}
